using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsCommand : ICommand<SettingsResponseDto>
    {
        public int Id { get; set; }
        public SettingsRequestDto Request { get; set; } = new();
    }
}