using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Branch;

namespace Alwalid.Cms.Api.Features.Branch.Commands.UpdateBranch
{
    public class UpdateBranchCommandHandler : ICommandHandler<UpdateBranchCommand, Entities.Branch>
    {
        private readonly IBranchRepository _branchRepository;

        public UpdateBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<Entities.Branch>> Handle(UpdateBranchCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if branch exists
                var existingBranch = await _branchRepository.GetByIdAsync(command.Id);
                if (existingBranch == null)
                {
                    return await Result<Entities.Branch>.FaildAsync(false, "Branch not found.");
                }

                // Validate unique constraints
                if (await _branchRepository.ExistsInCountryAsync(command.CountryId, command.City, command.Address, command.Id))
                {
                    return await Result<Entities.Branch>.FaildAsync(false, "Branch already exists in this country with the same city and address.");
                }

                // Update branch
                existingBranch.City = command.City;
                existingBranch.Address = command.Address;
                existingBranch.CountryId = command.CountryId;

                var updatedBranch = await _branchRepository.UpdateAsync(existingBranch);

                return await Result<Entities.Branch>.SuccessAsync(updatedBranch, "Branch updated successfully.");
            }
            catch (Exception ex)
            {
                return await Result<Entities.Branch>.FaildAsync(false, $"Error updating branch: {ex.Message}");
            }
        }
    }
} 