using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Category.Commands.AddCategory;
using Alwalid.Cms.Api.Features.Category.Commands.UpdateCategory;
using Alwalid.Cms.Api.Features.Category.Commands.DeleteCategory;
using Alwalid.Cms.Api.Features.Category.Commands.SoftDeleteCategory;
using Alwalid.Cms.Api.Features.Category.Queries.GetAllCategories;
using Alwalid.Cms.Api.Features.Category.Queries.GetCategoryById;
using Alwalid.Cms.Api.Features.Category.Queries.GetActiveCategories;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Alwalid.Cms.Api.Abstractions;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ODataController
    {
        private readonly IDispatcher _dispatcher;
        private readonly ICommandHandler<AddCategoryCommand, CategoryResponseDto> _addCategoryHandler;
        private readonly ICommandHandler<UpdateCategoryCommand, CategoryResponseDto> _updateCategoryHandler;
        private readonly ICommandHandler<DeleteCategoryCommand, bool> _deleteCategoryHandler;
        private readonly ICommandHandler<SoftDeleteCategoryCommand, bool> _softDeleteCategoryHandler;
        private readonly IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponseDto>> _getAllCategoriesHandler;
        private readonly IQueryHandler<GetCategoryByIdQuery, CategoryResponseDto> _getCategoryByIdHandler;
        private readonly IQueryHandler<GetActiveCategoriesQuery, IEnumerable<CategoryResponseDto>> _getActiveCategoriesHandler;

        public CategoryController(
            ICommandHandler<AddCategoryCommand, CategoryResponseDto> addCategoryHandler,
            ICommandHandler<UpdateCategoryCommand, CategoryResponseDto> updateCategoryHandler,
            ICommandHandler<DeleteCategoryCommand, bool> deleteCategoryHandler,
            ICommandHandler<SoftDeleteCategoryCommand, bool> softDeleteCategoryHandler,
            IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponseDto>> getAllCategoriesHandler,
            IQueryHandler<GetCategoryByIdQuery, CategoryResponseDto> getCategoryByIdHandler,
            IQueryHandler<GetActiveCategoriesQuery, IEnumerable<CategoryResponseDto>> getActiveCategoriesHandler,
            IDispatcher dispatcher)
        {
            _addCategoryHandler = addCategoryHandler;
            _updateCategoryHandler = updateCategoryHandler;
            _deleteCategoryHandler = deleteCategoryHandler;
            _softDeleteCategoryHandler = softDeleteCategoryHandler;
            _getAllCategoriesHandler = getAllCategoriesHandler;
            _getCategoryByIdHandler = getCategoryByIdHandler;
            _getActiveCategoriesHandler = getActiveCategoriesHandler;
            _dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddCategoryCommand
            {
                Request = request,
            };
            //var  result= await _addCategoryHandler.Handle(command, cancellationToken);
            var result = await _dispatcher.SendCommandAsync(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateCategoryCommand
            {
                Id = id,
                Request = request
            };
            var result = await _updateCategoryHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryCommand{ Id = id};
            var result = await _deleteCategoryHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteCategory(int id, CancellationToken cancellationToken)
        {
            var command = new SoftDeleteCategoryCommand
            {
                Id= id,
            };
            var result = await _softDeleteCategoryHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var query = new GetAllCategoriesQuery();
            //var result = await _getAllCategoriesHandler.Handle(query, cancellationToken);
            var result = await _dispatcher.SendQueryAsync(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var query = new GetCategoryByIdQuery
            {
                Id = id,
            };
            //var result = await _getCategoryByIdHandler.Handle(query, cancellationToken);
            var result = await _dispatcher.SendQueryAsync(query, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveCategories(CancellationToken cancellationToken)
        {
            var query = new GetActiveCategoriesQuery();
            var result = await _getActiveCategoriesHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }
    }
}