using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Settings.Commands.AddSettings;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateSettings;
using Alwalid.Cms.Api.Features.Settings.Commands.DeleteSettings;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultCurrency;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateMaintenanceMode;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultLanguage;
using Alwalid.Cms.Api.Features.Settings.Queries.GetAllSettings;
using Alwalid.Cms.Api.Features.Settings.Queries.GetSettingsById;
using Alwalid.Cms.Api.Features.Settings.Queries.GetMainSettings;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ODataController
    {
        private readonly ICommandHandler<AddSettingsCommand, SettingsResponseDto> _addSettingsHandler;
        private readonly ICommandHandler<UpdateSettingsCommand, SettingsResponseDto> _updateSettingsHandler;
        private readonly ICommandHandler<DeleteSettingsCommand, bool> _deleteSettingsHandler;
        private readonly ICommandHandler<UpdateDefaultCurrencyCommand, bool> _updateDefaultCurrencyHandler;
        private readonly ICommandHandler<UpdateMaintenanceModeCommand, bool> _updateMaintenanceModeHandler;
        private readonly ICommandHandler<UpdateDefaultLanguageCommand, bool> _updateDefaultLanguageHandler;
        private readonly IQueryHandler<GetAllSettingsQuery, IEnumerable<SettingsResponseDto>> _getAllSettingsHandler;
        private readonly IQueryHandler<GetSettingsByIdQuery, SettingsResponseDto> _getSettingsByIdHandler;
        private readonly IQueryHandler<GetMainSettingsQuery, SettingsResponseDto> _getMainSettingsHandler;

        public SettingsController(
            ICommandHandler<AddSettingsCommand, SettingsResponseDto> addSettingsHandler,
            ICommandHandler<UpdateSettingsCommand, SettingsResponseDto> updateSettingsHandler,
            ICommandHandler<DeleteSettingsCommand, bool> deleteSettingsHandler,
            ICommandHandler<UpdateDefaultCurrencyCommand, bool> updateDefaultCurrencyHandler,
            ICommandHandler<UpdateMaintenanceModeCommand, bool> updateMaintenanceModeHandler,
            ICommandHandler<UpdateDefaultLanguageCommand, bool> updateDefaultLanguageHandler,
            IQueryHandler<GetAllSettingsQuery, IEnumerable<SettingsResponseDto>> getAllSettingsHandler,
            IQueryHandler<GetSettingsByIdQuery, SettingsResponseDto> getSettingsByIdHandler,
            IQueryHandler<GetMainSettingsQuery, SettingsResponseDto> getMainSettingsHandler)
        {
            _addSettingsHandler = addSettingsHandler;
            _updateSettingsHandler = updateSettingsHandler;
            _deleteSettingsHandler = deleteSettingsHandler;
            _updateDefaultCurrencyHandler = updateDefaultCurrencyHandler;
            _updateMaintenanceModeHandler = updateMaintenanceModeHandler;
            _updateDefaultLanguageHandler = updateDefaultLanguageHandler;
            _getAllSettingsHandler = getAllSettingsHandler;
            _getSettingsByIdHandler = getSettingsByIdHandler;
            _getMainSettingsHandler = getMainSettingsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddSettings([FromBody] SettingsRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddSettingsCommand
            {
                Request = request
            };
            var result = await _addSettingsHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSettings(int id, [FromBody] SettingsRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateSettingsCommand
            {
                Id = id,
                Request = request
            };
            var result = await _updateSettingsHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettings(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteSettingsCommand
            {
                Id = id
            };
            var result = await _deleteSettingsHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPatch("{id}/default-currency")]
        public async Task<IActionResult> UpdateDefaultCurrency(int id, [FromBody] DefaultCurrencyRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateDefaultCurrencyCommand
            {
                Id = id,
                Request = request,
            };
            var result = await _updateDefaultCurrencyHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPatch("{id}/maintenance-mode")]
        public async Task<IActionResult> UpdateMaintenanceMode(int id, [FromBody] MaintenanceModeRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateMaintenanceModeCommand
            {
                Id= id,
                Request = request,
            };
            var result = await _updateMaintenanceModeHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPatch("{id}/default-language")]
        public async Task<IActionResult> UpdateDefaultLanguage(int id, [FromBody] DefaultLanguageRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateDefaultLanguageCommand
            {
                Request = request,
                Id = id,
            };
            var result = await _updateDefaultLanguageHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllSettings(CancellationToken cancellationToken)
        {
            var query = new GetAllSettingsQuery();
            var result = await _getAllSettingsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSettingsById(int id, CancellationToken cancellationToken)
        {
            var query = new GetSettingsByIdQuery
            {
                Id = id
            };
            var result = await _getSettingsByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("main")]
        public async Task<IActionResult> GetMainSettings(CancellationToken cancellationToken)
        {
            var query = new GetMainSettingsQuery();
            var result = await _getMainSettingsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }
    }
}