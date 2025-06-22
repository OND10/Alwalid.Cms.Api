using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateMaintenanceMode
{
    public class UpdateMaintenanceModeCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public MaintenanceModeRequestDto Request {  get; set; }
    }
}