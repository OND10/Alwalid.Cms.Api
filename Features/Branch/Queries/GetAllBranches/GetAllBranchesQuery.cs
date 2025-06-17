using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Branch.Queries.GetAllBranches
{
    public class GetAllBranchesQuery : IQuery<IEnumerable<Entities.Branch>>
    {
        // No parameters needed for getting all branches
    }
} 