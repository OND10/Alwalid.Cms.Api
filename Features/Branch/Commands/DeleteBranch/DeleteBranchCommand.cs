using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Branch.Commands.DeleteBranch
{
    public class DeleteBranchCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 