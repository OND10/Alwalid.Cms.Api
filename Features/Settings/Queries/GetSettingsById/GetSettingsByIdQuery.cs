using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Queries.GetSettingsById
{
    public class GetSettingsByIdQuery : IQuery<SettingsResponseDto?>
    {
        public int Id { get; set; }
    }
}