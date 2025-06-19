using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Branch;

namespace Alwalid.Cms.Api.Features.Branch.Queries.GetBranchById
{
    public class GetBranchByIdQueryHandler : IQueryHandler<GetBranchByIdQuery, Entities.Branch>
    {
        private readonly IBranchRepository _branchRepository;

        public GetBranchByIdQueryHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<Entities.Branch>> Handle(GetBranchByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var branch = await _branchRepository.GetByIdAsync(query.Id);
                
                if (branch == null)
                {
                    return await Result<Entities.Branch>.FaildAsync(false, "Branch not found.");
                }

                return await Result<Entities.Branch>.SuccessAsync(branch, "Branch retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<Entities.Branch>.FaildAsync(false, $"Error retrieving branch: {ex.Message}");
            }
        }
    }
} 