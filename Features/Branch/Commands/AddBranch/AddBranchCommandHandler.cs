using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Branch;
using Alwalid.Cms.Api.Features.Branch.Dtos;

namespace Alwalid.Cms.Api.Features.Branch.Commands.AddBranch
{
    public class AddBranchCommandHandler : ICommandHandler<AddBranchCommand, BranchResponseDto>
    {
        private readonly IBranchRepository _branchRepository;

        public AddBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<BranchResponseDto>> Handle(AddBranchCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Validate unique constraints
                if (await _branchRepository.ExistsInCountryAsync(command.Request.CountryId, command.Request.City, command.Request.Address))
                {
                    return await Result<BranchResponseDto>.FaildAsync(false, "Branch already exists in this country with the same city and address.");
                }

                // Create new branch
                var branch = new Entities.Branch
                {
                    City = command.Request.City,
                    Address = command.Request.Address,
                    CountryId = command.Request.CountryId
                };

                var createdBranch = await _branchRepository.CreateAsync(branch);

                // Map to response DTO
                var responseDto = new BranchResponseDto
                {
                    Id = createdBranch.Id,
                    City = createdBranch.City,
                    Address = createdBranch.Address,
                    CountryId = createdBranch.CountryId,
                    CountryName = createdBranch.Country?.Name ?? string.Empty,
                    CountryCode = createdBranch.Country?.Code ?? string.Empty,
                    DepartmentsCount = createdBranch.Departments?.Count ?? 0
                };

                return await Result<BranchResponseDto>.SuccessAsync(responseDto, "Branch created successfully.");
            }
            catch (Exception ex)
            {
                return await Result<BranchResponseDto>.FaildAsync(false, $"Error creating branch: {ex.Message}");
            }
        }
    }
} 