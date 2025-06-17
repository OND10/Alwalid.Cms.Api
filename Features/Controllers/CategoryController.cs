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

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ODataController
    {
        private readonly ICommandHandler<AddCategoryCommand, Result<int>> _addCategoryHandler;
        private readonly ICommandHandler<UpdateCategoryCommand, Result<bool>> _updateCategoryHandler;
        private readonly ICommandHandler<DeleteCategoryCommand, Result<bool>> _deleteCategoryHandler;
        private readonly ICommandHandler<SoftDeleteCategoryCommand, Result<bool>> _softDeleteCategoryHandler;
        private readonly IQueryHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>> _getAllCategoriesHandler;
        private readonly IQueryHandler<GetCategoryByIdQuery, Result<GetCategoryByIdResponse>> _getCategoryByIdHandler;
        private readonly IQueryHandler<GetActiveCategoriesQuery, Result<List<GetActiveCategoriesResponse>>> _getActiveCategoriesHandler;

        public CategoryController(
            ICommandHandler<AddCategoryCommand, Result<int>> addCategoryHandler,
            ICommandHandler<UpdateCategoryCommand, Result<bool>> updateCategoryHandler,
            ICommandHandler<DeleteCategoryCommand, Result<bool>> deleteCategoryHandler,
            ICommandHandler<SoftDeleteCategoryCommand, Result<bool>> softDeleteCategoryHandler,
            IQueryHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>> getAllCategoriesHandler,
            IQueryHandler<GetCategoryByIdQuery, Result<GetCategoryByIdResponse>> getCategoryByIdHandler,
            IQueryHandler<GetActiveCategoriesQuery, Result<List<GetActiveCategoriesResponse>>> getActiveCategoriesHandler)
        {
            _addCategoryHandler = addCategoryHandler;
            _updateCategoryHandler = updateCategoryHandler;
            _deleteCategoryHandler = deleteCategoryHandler;
            _softDeleteCategoryHandler = softDeleteCategoryHandler;
            _getAllCategoriesHandler = getAllCategoriesHandler;
            _getCategoryByIdHandler = getCategoryByIdHandler;
            _getActiveCategoriesHandler = getActiveCategoriesHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto request)
        {
            var command = new AddCategoryCommand(request);
            var result = await _addCategoryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand(id, request);
            var result = await _updateCategoryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _deleteCategoryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteCategory(int id)
        {
            var command = new SoftDeleteCategoryCommand(id);
            var result = await _softDeleteCategoryHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _getAllCategoriesHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _getCategoryByIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveCategories()
        {
            var query = new GetActiveCategoriesQuery();
            var result = await _getActiveCategoriesHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }
    }
} 