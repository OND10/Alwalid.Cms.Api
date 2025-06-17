using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Currency.Commands.AddCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.UpdateCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.DeleteCurrency;
using Alwalid.Cms.Api.Features.Currency.Queries.GetAllCurrencies;
using Alwalid.Cms.Api.Features.Currency.Queries.GetCurrencyById;
using Alwalid.Cms.Api.Features.Currency.Queries.GetActiveCurrencies;
using Alwalid.Cms.Api.Features.Currency.Queries.GetBySymbol;
using Alwalid.Cms.Api.Features.Currency.Queries.GetByName;
using Alwalid.Cms.Api.Features.Currency.Queries.GetByCode;
using Alwalid.Cms.Api.Features.Currency.Queries.GetTotalCount;
using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ODataController
    {
        private readonly ICommandHandler<AddCurrencyCommand, Result<int>> _addCurrencyHandler;
        private readonly ICommandHandler<UpdateCurrencyCommand, Result<bool>> _updateCurrencyHandler;
        private readonly ICommandHandler<DeleteCurrencyCommand, Result<bool>> _deleteCurrencyHandler;
        private readonly IQueryHandler<GetAllCurrenciesQuery, Result<List<GetAllCurrenciesResponse>>> _getAllCurrenciesHandler;
        private readonly IQueryHandler<GetCurrencyByIdQuery, Result<GetCurrencyByIdResponse>> _getCurrencyByIdHandler;
        private readonly IQueryHandler<GetActiveCurrenciesQuery, Result<List<GetActiveCurrenciesResponse>>> _getActiveCurrenciesHandler;
        private readonly IQueryHandler<GetBySymbolQuery, Result<GetBySymbolResponse>> _getBySymbolHandler;
        private readonly IQueryHandler<GetByNameQuery, Result<GetByNameResponse>> _getByNameHandler;
        private readonly IQueryHandler<GetByCodeQuery, Result<GetByCodeResponse>> _getByCodeHandler;
        private readonly IQueryHandler<GetTotalCountQuery, Result<int>> _getTotalCountHandler;

        public CurrencyController(
            ICommandHandler<AddCurrencyCommand, Result<int>> addCurrencyHandler,
            ICommandHandler<UpdateCurrencyCommand, Result<bool>> updateCurrencyHandler,
            ICommandHandler<DeleteCurrencyCommand, Result<bool>> deleteCurrencyHandler,
            IQueryHandler<GetAllCurrenciesQuery, Result<List<GetAllCurrenciesResponse>>> getAllCurrenciesHandler,
            IQueryHandler<GetCurrencyByIdQuery, Result<GetCurrencyByIdResponse>> getCurrencyByIdHandler,
            IQueryHandler<GetActiveCurrenciesQuery, Result<List<GetActiveCurrenciesResponse>>> getActiveCurrenciesHandler,
            IQueryHandler<GetBySymbolQuery, Result<GetBySymbolResponse>> getBySymbolHandler,
            IQueryHandler<GetByNameQuery, Result<GetByNameResponse>> getByNameHandler,
            IQueryHandler<GetByCodeQuery, Result<GetByCodeResponse>> getByCodeHandler,
            IQueryHandler<GetTotalCountQuery, Result<int>> getTotalCountHandler)
        {
            _addCurrencyHandler = addCurrencyHandler;
            _updateCurrencyHandler = updateCurrencyHandler;
            _deleteCurrencyHandler = deleteCurrencyHandler;
            _getAllCurrenciesHandler = getAllCurrenciesHandler;
            _getCurrencyByIdHandler = getCurrencyByIdHandler;
            _getActiveCurrenciesHandler = getActiveCurrenciesHandler;
            _getBySymbolHandler = getBySymbolHandler;
            _getByNameHandler = getByNameHandler;
            _getByCodeHandler = getByCodeHandler;
            _getTotalCountHandler = getTotalCountHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrency([FromBody] AddCurrencyRequest request)
        {
            var command = new AddCurrencyCommand(request);
            var result = await _addCurrencyHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurrency(int id, [FromBody] UpdateCurrencyRequest request)
        {
            var command = new UpdateCurrencyCommand(id, request);
            var result = await _updateCurrencyHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            var command = new DeleteCurrencyCommand(id);
            var result = await _deleteCurrencyHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var query = new GetAllCurrenciesQuery();
            var result = await _getAllCurrenciesHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id)
        {
            var query = new GetCurrencyByIdQuery(id);
            var result = await _getCurrencyByIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveCurrencies()
        {
            var query = new GetActiveCurrenciesQuery();
            var result = await _getActiveCurrenciesHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("symbol/{symbol}")]
        public async Task<IActionResult> GetBySymbol(string symbol)
        {
            var query = new GetBySymbolQuery(symbol);
            var result = await _getBySymbolHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var query = new GetByNameQuery(name);
            var result = await _getByNameHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var query = new GetByCodeQuery(code);
            var result = await _getByCodeHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetTotalCount()
        {
            var query = new GetTotalCountQuery();
            var result = await _getTotalCountHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }
    }
} 