using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Branch;

namespace Alwalid.Cms.Api.Features.Branch.Commands.DeleteBranch
{
    public class DeleteBranchCommandHandler : ICommandHandler<DeleteBranchCommand, bool>
    {
        private readonly IBranchRepository _branchRepository;

        public DeleteBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<bool>> Handle(DeleteBranchCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if branch exists
                if (!await _branchRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Branch not found.");
                }

                // Delete branch
                var isDeleted = await _branchRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    return await Result<bool>.SuccessAsync(true, "Branch deleted successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete branch.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting branch: {ex.Message}");
            }
        }
    }
} 