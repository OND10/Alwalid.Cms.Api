using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Features.Currency.Queries.GetAllCurrencies;
using Alwalid.Cms.Api.Features.Partners.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageRepository _imageRepository;
        public PartnerController(ApplicationDbContext context, IImageRepository imageRepository)
        {
            _context = context;
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddPartner([FromBody] PartnerRequestDto request, CancellationToken cancellationToken)
        {

            var model = new Entities.Partners
            {
                Email = request.Email,
                ArabicDescription = request.ArabicDescription,
                ArabicName = request.ArabicName,
                Description = request.Description,
                Location = request.Location,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
            };

            var imageUrl = await _imageRepository.Upload(model, request.file);

            model.ImageUrl = imageUrl;

            var result = await _context.Partners.AddAsync(model);

            await _context.SaveChangesAsync();

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePartner(int id, [FromBody] PartnerRequestDto request, CancellationToken cancellationToken)
        {
            var model = new Entities.Partners
            {
                Id = id,
                PhoneNumber = request.PhoneNumber,
                Name = request.Name,
                Location = request.Location,
                Description = request.Description,
                ArabicDescription = request.ArabicDescription,
                ArabicName = request.ArabicName,
                Email = request.Email,
            };

            

            var result = _context.Partners.Update(model);

            await _context.SaveChangesAsync();

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartner(int id, CancellationToken cancellationToken)
        {

            var model = await _context.Partners.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (model == null)
            {

                return BadRequest(model);
            }

            var result = _context.Partners.Remove(model);


            return Ok(result);

        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCurrencies(CancellationToken cancellationToken)
        {
            var query = new GetAllCurrenciesQuery();
            var result = await _context.Partners.ToListAsync();

            if (result.Any())
            {
                return BadRequest(result);

            }
            return Ok(result.AsQueryable());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrencyById(int id, CancellationToken cancellationToken)
        {
            var result = await _context.Partners.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound(result);

            }
            return Ok(result);
        }

    }
}
