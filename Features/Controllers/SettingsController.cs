//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Alwalid.Cms.Api.Features.Settings.Commands.AddSettings;
//using Alwalid.Cms.Api.Features.Settings.Commands.UpdateSettings;
//using Alwalid.Cms.Api.Features.Settings.Commands.DeleteSettings;
//using Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultCurrency;
//using Alwalid.Cms.Api.Features.Settings.Commands.UpdateMaintenanceMode;
//using Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultLanguage;
//using Alwalid.Cms.Api.Features.Settings.Queries.GetAllSettings;
//using Alwalid.Cms.Api.Features.Settings.Queries.GetSettingsById;
//using Alwalid.Cms.Api.Features.Settings.Queries.GetMainSettings;
//using Alwalid.Cms.Api.Abstractions.Messaging;

//namespace Alwalid.Cms.Api.Features.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class SettingsController : ODataController
//    {
//        private readonly ICommandHandler<AddSettingsCommand, Result<int>> _addSettingsHandler;
//        private readonly ICommandHandler<UpdateSettingsCommand, Result<bool>> _updateSettingsHandler;
//        private readonly ICommandHandler<DeleteSettingsCommand, Result<bool>> _deleteSettingsHandler;
//        private readonly ICommandHandler<UpdateDefaultCurrencyCommand, Result<bool>> _updateDefaultCurrencyHandler;
//        private readonly ICommandHandler<UpdateMaintenanceModeCommand, Result<bool>> _updateMaintenanceModeHandler;
//        private readonly ICommandHandler<UpdateDefaultLanguageCommand, Result<bool>> _updateDefaultLanguageHandler;
//        private readonly IQueryHandler<GetAllSettingsQuery, Result<List<GetAllSettingsResponse>>> _getAllSettingsHandler;
//        private readonly IQueryHandler<GetSettingsByIdQuery, Result<GetSettingsByIdResponse>> _getSettingsByIdHandler;
//        private readonly IQueryHandler<GetMainSettingsQuery, Result<GetMainSettingsResponse>> _getMainSettingsHandler;

//        public SettingsController(
//            ICommandHandler<AddSettingsCommand, Result<int>> addSettingsHandler,
//            ICommandHandler<UpdateSettingsCommand, Result<bool>> updateSettingsHandler,
//            ICommandHandler<DeleteSettingsCommand, Result<bool>> deleteSettingsHandler,
//            ICommandHandler<UpdateDefaultCurrencyCommand, Result<bool>> updateDefaultCurrencyHandler,
//            ICommandHandler<UpdateMaintenanceModeCommand, Result<bool>> updateMaintenanceModeHandler,
//            ICommandHandler<UpdateDefaultLanguageCommand, Result<bool>> updateDefaultLanguageHandler,
//            IQueryHandler<GetAllSettingsQuery, Result<List<GetAllSettingsResponse>>> getAllSettingsHandler,
//            IQueryHandler<GetSettingsByIdQuery, Result<GetSettingsByIdResponse>> getSettingsByIdHandler,
//            IQueryHandler<GetMainSettingsQuery, Result<GetMainSettingsResponse>> getMainSettingsHandler)
//        {
//            _addSettingsHandler = addSettingsHandler;
//            _updateSettingsHandler = updateSettingsHandler;
//            _deleteSettingsHandler = deleteSettingsHandler;
//            _updateDefaultCurrencyHandler = updateDefaultCurrencyHandler;
//            _updateMaintenanceModeHandler = updateMaintenanceModeHandler;
//            _updateDefaultLanguageHandler = updateDefaultLanguageHandler;
//            _getAllSettingsHandler = getAllSettingsHandler;
//            _getSettingsByIdHandler = getSettingsByIdHandler;
//            _getMainSettingsHandler = getMainSettingsHandler;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddSettings([FromBody] AddSettingsRequest request)
//        {
//            var command = new AddSettingsCommand(request);
//            var result = await _addSettingsHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateSettings(int id, [FromBody] UpdateSettingsRequest request)
//        {
//            var command = new UpdateSettingsCommand(id, request);
//            var result = await _updateSettingsHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteSettings(int id)
//        {
//            var command = new DeleteSettingsCommand(id);
//            var result = await _deleteSettingsHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpPatch("{id}/default-currency")]
//        public async Task<IActionResult> UpdateDefaultCurrency(int id, [FromBody] UpdateDefaultCurrencyRequest request)
//        {
//            var command = new UpdateDefaultCurrencyCommand(id, request);
//            var result = await _updateDefaultCurrencyHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpPatch("{id}/maintenance-mode")]
//        public async Task<IActionResult> UpdateMaintenanceMode(int id, [FromBody] UpdateMaintenanceModeRequest request)
//        {
//            var command = new UpdateMaintenanceModeCommand(id, request);
//            var result = await _updateMaintenanceModeHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpPatch("{id}/default-language")]
//        public async Task<IActionResult> UpdateDefaultLanguage(int id, [FromBody] UpdateDefaultLanguageRequest request)
//        {
//            var command = new UpdateDefaultLanguageCommand(id, request);
//            var result = await _updateDefaultLanguageHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpGet]
//        [EnableQuery]
//        public async Task<IActionResult> GetAllSettings()
//        {
//            var query = new GetAllSettingsQuery();
//            var result = await _getAllSettingsHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value.AsQueryable());
            
//            return BadRequest(result.Error);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetSettingsById(int id)
//        {
//            var query = new GetSettingsByIdQuery(id);
//            var result = await _getSettingsByIdHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return NotFound(result.Error);
//        }

//        [HttpGet("main")]
//        public async Task<IActionResult> GetMainSettings()
//        {
//            var query = new GetMainSettingsQuery();
//            var result = await _getMainSettingsHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }
//    }
//} 