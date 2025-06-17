using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Branch.Dtos;

namespace Alwalid.Cms.Api.Features.Branch.Commands.AddBranch
{
    public class AddBranchCommand : ICommand<BranchResponseDto>
    {
        public BranchRequestDto Request { get; set; } = new();
    }
} 