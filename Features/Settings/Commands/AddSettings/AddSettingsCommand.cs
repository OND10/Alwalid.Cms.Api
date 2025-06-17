using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.AddSettings
{
    public class AddSettingsCommand : ICommand<SettingsResponseDto>
    {
        public SettingsRequestDto Request { get; set; } = new();
    }
} 