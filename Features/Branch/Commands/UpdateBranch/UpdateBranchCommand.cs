using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Branch.Commands.UpdateBranch
{
    public class UpdateBranchCommand : ICommand<Entities.Branch>
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
} 