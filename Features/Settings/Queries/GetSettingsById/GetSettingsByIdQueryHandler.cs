using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Queries.GetSettingsById
{
    public class GetSettingsByIdQueryHandler : IQueryHandler<GetSettingsByIdQuery, SettingsResponseDto?>
    {
        private readonly ISettingsRepository _settingsRepository;

        public GetSettingsByIdQueryHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<SettingsResponseDto?>> Handle(GetSettingsByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var settings = await _settingsRepository.GetByIdAsync(query.Id);

                if (settings == null)
                {
                    return await Result<SettingsResponseDto?>.FaildAsync(false, "Settings not found.");
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

                return await Result<SettingsResponseDto?>.SuccessAsync(responseDto, "Settings retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<SettingsResponseDto?>.FaildAsync(false, $"Error retrieving settings: {ex.Message}");
            }
        }
    }
}