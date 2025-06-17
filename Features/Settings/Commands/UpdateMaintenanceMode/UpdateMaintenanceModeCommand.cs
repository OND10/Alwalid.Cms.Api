using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateMaintenanceMode
{
    public class UpdateMaintenanceModeCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public bool MaintenanceMode { get; set; }
    }
}