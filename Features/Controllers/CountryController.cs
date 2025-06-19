using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Country.Commands.AddCountry;
using Alwalid.Cms.Api.Features.Country.Commands.UpdateCountry;
using Alwalid.Cms.Api.Features.Country.Commands.DeleteCountry;
using Alwalid.Cms.Api.Features.Country.Queries.GetAllCountries;
using Alwalid.Cms.Api.Features.Country.Queries.GetCountryById;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ODataController
    {
        private readonly ICommandHandler<AddCountryCommand, CountryResponseDto> _addCountryHandler;
        private readonly ICommandHandler<UpdateCountryCommand, CountryResponseDto> _updateCountryHandler;
        private readonly ICommandHandler<DeleteCountryCommand, bool> _deleteCountryHandler;
        private readonly IQueryHandler<GetAllCountriesQuery, IEnumerable<CountryResponseDto>> _getAllCountriesHandler;
        private readonly IQueryHandler<GetCountryByIdQuery, CountryResponseDto> _getCountryByIdHandler;

        public CountryController(
            ICommandHandler<AddCountryCommand, CountryResponseDto> addCountryHandler,
            ICommandHandler<UpdateCountryCommand, CountryResponseDto> updateCountryHandler,
            ICommandHandler<DeleteCountryCommand, bool> deleteCountryHandler,
            IQueryHandler<GetAllCountriesQuery, IEnumerable<CountryResponseDto>> getAllCountriesHandler,
            IQueryHandler<GetCountryByIdQuery, CountryResponseDto> getCountryByIdHandler)
        {
            _addCountryHandler = addCountryHandler;
            _updateCountryHandler = updateCountryHandler;
            _deleteCountryHandler = deleteCountryHandler;
            _getAllCountriesHandler = getAllCountriesHandler;
            _getCountryByIdHandler = getCountryByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] CountryRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddCountryCommand
            {
                Request = request,
            };

            var result = await _addCountryHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateCountryCommand
            {
                Id = id,
                Request = request.Request
            };
            var result = await _updateCountryHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteCountryCommand
            {
                Id = id
            };
            var result = await _deleteCountryHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken)
        {
            var query = new GetAllCountriesQuery();
            var result = await _getAllCountriesHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountryById(int id, CancellationToken cancellationToken)
        {
            var query = new GetCountryByIdQuery
            {
                Id = id
            };
            var result = await _getCountryByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound();
        }
    }
}