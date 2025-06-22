using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Currency.Commands.AddCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.UpdateCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.DeleteCurrency;
using Alwalid.Cms.Api.Features.Currency.Queries.GetAllCurrencies;
using Alwalid.Cms.Api.Features.Currency.Queries.GetCurrencyById;
using Alwalid.Cms.Api.Features.Currency.Queries.GetActiveCurrencies;
//using Alwalid.Cms.Api.Features.Currency.Queries.GetBySymbol;
//using Alwalid.Cms.Api.Features.Currency.Queries.GetByName;
using Alwalid.Cms.Api.Features.Currency.Queries.GetByCode;
using Alwalid.Cms.Api.Features.Currency.Queries.GetTotalCount;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;
using Alwalid.Cms.Api.Features.Currency.Queries.GetBySymbol;
using Alwalid.Cms.Api.Features.Currency.Queries.GetByName;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ODataController
    {
        private readonly ICommandHandler<AddCurrencyCommand, CurrencyResponseDto> _addCurrencyHandler;
        private readonly ICommandHandler<UpdateCurrencyCommand, CurrencyResponseDto> _updateCurrencyHandler;
        private readonly ICommandHandler<DeleteCurrencyCommand, bool> _deleteCurrencyHandler;
        private readonly IQueryHandler<GetAllCurrenciesQuery, IEnumerable<CurrencyResponseDto>> _getAllCurrenciesHandler;
        private readonly IQueryHandler<GetCurrencyByIdQuery, CurrencyResponseDto> _getCurrencyByIdHandler;
        private readonly IQueryHandler<GetActiveCurrenciesQuery, IEnumerable<CurrencyResponseDto>> _getActiveCurrenciesHandler;
        private readonly IQueryHandler<GetBySymbolQuery, CurrencyResponseDto> _getBySymbolHandler;
        private readonly IQueryHandler<GetByNameQuery, CurrencyResponseDto> _getByNameHandler;
        private readonly IQueryHandler<GetByCodeQuery, CurrencyResponseDto> _getByCodeHandler;
        private readonly IQueryHandler<GetTotalCountQuery, int> _getTotalCountHandler;

        public CurrencyController(
            ICommandHandler<AddCurrencyCommand, CurrencyResponseDto> addCurrencyHandler,
            ICommandHandler<UpdateCurrencyCommand, CurrencyResponseDto> updateCurrencyHandler,
            ICommandHandler<DeleteCurrencyCommand, bool> deleteCurrencyHandler,
            IQueryHandler<GetAllCurrenciesQuery, IEnumerable<CurrencyResponseDto>> getAllCurrenciesHandler,
            IQueryHandler<GetCurrencyByIdQuery, CurrencyResponseDto> getCurrencyByIdHandler,
            IQueryHandler<GetActiveCurrenciesQuery, IEnumerable<CurrencyResponseDto>> getActiveCurrenciesHandler,
            IQueryHandler<GetBySymbolQuery, CurrencyResponseDto> getBySymbolHandler,
            IQueryHandler<GetByNameQuery, CurrencyResponseDto> getByNameHandler,
            IQueryHandler<GetByCodeQuery, CurrencyResponseDto> getByCodeHandler,
            IQueryHandler<GetTotalCountQuery, int> getTotalCountHandler)
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
        public async Task<IActionResult> AddCurrency([FromBody] CurrencyRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddCurrencyCommand
            {
                Request = request,
            };

            var result = await _addCurrencyHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurrency(int id, [FromBody] CurrencyRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateCurrencyCommand
            {
                Request = request,
                Id = id
            };

            var result = await _updateCurrencyHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteCurrencyCommand
            {
                Id = id
            };
            var result = await _deleteCurrencyHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCurrencies(CancellationToken cancellationToken)
        {
            var query = new GetAllCurrenciesQuery();
            var result = await _getAllCurrenciesHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id, CancellationToken cancellationToken)
        {
            var query = new GetCurrencyByIdQuery
            {
                Id = id
            };
            var result = await _getCurrencyByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveCurrencies(CancellationToken cancellationToken)
        {
            var query = new GetActiveCurrenciesQuery();
            var result = await _getActiveCurrenciesHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("symbol/{symbol}")]
        public async Task<IActionResult> GetBySymbol(string symbol, CancellationToken cancellationToken)
        {
            var query = new GetBySymbolQuery
            {
                Symbol = symbol
            };
            var result = await _getBySymbolHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var query = new GetByNameQuery
            {
                Name = name
            };
            var result = await _getByNameHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken)
        {
            var query = new GetByCodeQuery
            {
                Code = code
            };
            var result = await _getByCodeHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetTotalCount(CancellationToken cancellationToken)
        {
            var query = new GetTotalCountQuery();
            var result = await _getTotalCountHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }
    }
}