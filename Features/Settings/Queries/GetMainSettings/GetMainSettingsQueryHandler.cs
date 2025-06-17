using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Queries.GetMainSettings
{
    public class GetMainSettingsQueryHandler : IQueryHandler<GetMainSettingsQuery, SettingsResponseDto?>
    {
        private readonly ISettingsRepository _settingsRepository;

        public GetMainSettingsQueryHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<SettingsResponseDto?>> Handle(GetMainSettingsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var settings = await _settingsRepository.GetMainSettingsAsync();

                if (settings == null)
                {
                    return await Result<SettingsResponseDto?>.FaildAsync(false, "Main settings not found.");
                }

                var responseDto = new SettingsResponseDto
                {
                    Id = settings.Id,
                    DefaultLanguage = settings.DefaultLanguage,
                    //DefaultCurrencyId = settings.DefaultCurrencyId,
                    //MaintenanceMode = settings.MaintenanceMode,
                    //CreatedAt = settings.CreatedAt,
                    //LastModifiedAt = settings.LastModifiedAt,
                    //IsDeleted = settings.IsDeleted
                };

                return await Result<SettingsResponseDto?>.SuccessAsync(responseDto, "Main settings retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<SettingsResponseDto?>.FaildAsync(false, $"Error retrieving main settings: {ex.Message}");
            }
        }
    }
}