using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Branch;

namespace Alwalid.Cms.Api.Features.Branch.Queries.GetAllBranches
{
    public class GetAllBranchesQueryHandler : IQueryHandler<GetAllBranchesQuery, IEnumerable<Entities.Branch>>
    {
        private readonly IBranchRepository _branchRepository;

        public GetAllBranchesQueryHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Result<IEnumerable<Entities.Branch>>> Handle(GetAllBranchesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var branches = await _branchRepository.GetAllAsync();
                return await Result<IEnumerable<Entities.Branch>>.SuccessAsync(branches, "Branches retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<Entities.Branch>>.FaildAsync(false, $"Error retrieving branches: {ex.Message}");
            }
        }
    }
} 