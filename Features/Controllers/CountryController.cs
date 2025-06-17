using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Country.Commands.AddCountry;
using Alwalid.Cms.Api.Features.Country.Commands.UpdateCountry;
using Alwalid.Cms.Api.Features.Country.Commands.DeleteCountry;
using Alwalid.Cms.Api.Features.Country.Queries.GetAllCountries;
using Alwalid.Cms.Api.Features.Country.Queries.GetCountryById;
using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ODataController
    {
        private readonly ICommandHandler<AddCountryCommand, Result<int>> _addCountryHandler;
        private readonly ICommandHandler<UpdateCountryCommand, Result<bool>> _updateCountryHandler;
        private readonly ICommandHandler<DeleteCountryCommand, Result<bool>> _deleteCountryHandler;
        private readonly IQueryHandler<GetAllCountriesQuery, Result<List<GetAllCountriesResponse>>> _getAllCountriesHandler;
        private readonly IQueryHandler<GetCountryByIdQuery, Result<GetCountryByIdResponse>> _getCountryByIdHandler;

        public CountryController(
            ICommandHandler<AddCountryCommand, Result<int>> addCountryHandler,
            ICommandHandler<UpdateCountryCommand, Result<bool>> updateCountryHandler,
            ICommandHandler<DeleteCountryCommand, Result<bool>> deleteCountryHandler,
            IQueryHandler<GetAllCountriesQuery, Result<List<GetAllCountriesResponse>>> getAllCountriesHandler,
            IQueryHandler<GetCountryByIdQuery, Result<GetCountryByIdResponse>> getCountryByIdHandler)
        {
            _addCountryHandler = addCountryHandler;
            _updateCountryHandler = updateCountryHandler;
            _deleteCountryHandler = deleteCountryHandler;
            _getAllCountriesHandler = getAllCountriesHandler;
            _getCountryByIdHandler = getCountryByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] AddCountryRequest request)
        {
            var command = new AddCountryCommand(request);
            var result = await _addCountryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryRequest request)
        {
            var command = new UpdateCountryCommand(id, request);
            var result = await _updateCountryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var command = new DeleteCountryCommand(id);
            var result = await _deleteCountryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCountries()
        {
            var query = new GetAllCountriesQuery();
            var result = await _getAllCountriesHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var query = new GetCountryByIdQuery(id);
            var result = await _getCountryByIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }
    }
} 