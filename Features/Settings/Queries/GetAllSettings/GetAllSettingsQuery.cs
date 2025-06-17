using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Queries.GetAllSettings
{
    public class GetAllSettingsQuery : IQuery<IEnumerable<SettingsResponseDto>>
    {
        // No parameters needed for getting all settings
    }
}