using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Currency.Commands.AddCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.DeleteCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.UpdateCurrency;
using Alwalid.Cms.Api.Features.Currency.Dtos;
using Alwalid.Cms.Api.Features.Currency.Queries.GetAllCurrencies;
using Alwalid.Cms.Api.Features.Currency.Queries.GetCurrencyById;
using Alwalid.Cms.Api.Features.Services.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageRepository _imageRepository;
        public ServicesController(ApplicationDbContext context, IImageRepository imageRepository)
        {
            _context = context;
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddService([FromForm] ServiceRequestDto request, CancellationToken cancellationToken)
        {

            var model = new Entities.Services
            {
                Description = request.Description,
                Title = request.Title,

            };
            var imageUrl = await _imageRepository.Upload(model, request.file);

            model.ImageUrl = imageUrl;

            var result = await _context.Services.AddAsync(model);


            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                await _context.SaveChangesAsync();
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromForm] ServiceRequestDto request, CancellationToken cancellationToken)
        {
            var model = new Entities.Services
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
            };


            var result = _context.Services.Update(model);

            await _context.SaveChangesAsync();

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id, CancellationToken cancellationToken)
        {

            var model = await _context.Services.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (model == null)
            {

                return BadRequest(model);
            }

            var result = _context.Services.Remove(model);


            return Ok(result);

        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllServices(CancellationToken cancellationToken)
        {
            //var query = new GetAllCurrenciesQuery();
            var result = await _context.Services.ToListAsync();

            if (result.Any())
            {
                return Ok(result.AsQueryable());

            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id, CancellationToken cancellationToken)
        {
            var result = await _context.Services.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound(result);

            }
            return Ok(result);
        }

    }
}
