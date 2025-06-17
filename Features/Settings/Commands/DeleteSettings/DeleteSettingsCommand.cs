 using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Settings.Commands.DeleteSettings
{
    public class DeleteSettingsCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
}