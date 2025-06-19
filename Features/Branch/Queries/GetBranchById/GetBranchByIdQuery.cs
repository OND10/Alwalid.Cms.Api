using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Branch.Queries.GetBranchById
{
    public class GetBranchByIdQuery : IQuery<Entities.Branch>
    {
        public int Id { get; set; }
    }
} 