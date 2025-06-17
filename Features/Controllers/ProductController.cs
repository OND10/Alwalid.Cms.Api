using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Product.Commands.AddProduct;
using Alwalid.Cms.Api.Features.Product.Commands.UpdateProduct;
using Alwalid.Cms.Api.Features.Product.Commands.DeleteProduct;
using Alwalid.Cms.Api.Features.Product.Commands.SoftDeleteProduct;
using Alwalid.Cms.Api.Features.Product.Commands.UpdateStock;
using Alwalid.Cms.Api.Features.Product.Queries.GetAllProducts;
using Alwalid.Cms.Api.Features.Product.Queries.GetProductById;
using Alwalid.Cms.Api.Features.Product.Queries.GetActiveProducts;
using Alwalid.Cms.Api.Features.Product.Queries.GetLowStockProducts;
using Alwalid.Cms.Api.Features.Product.Queries.SearchProducts;
using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ODataController
    {
        private readonly ICommandHandler<AddProductCommand, Result<int>> _addProductHandler;
        private readonly ICommandHandler<UpdateProductCommand, Result<bool>> _updateProductHandler;
        private readonly ICommandHandler<DeleteProductCommand, Result<bool>> _deleteProductHandler;
        private readonly ICommandHandler<SoftDeleteProductCommand, Result<bool>> _softDeleteProductHandler;
        private readonly ICommandHandler<UpdateStockCommand, Result<bool>> _updateStockHandler;
        private readonly IQueryHandler<GetAllProductsQuery, Result<List<GetAllProductsResponse>>> _getAllProductsHandler;
        private readonly IQueryHandler<GetProductByIdQuery, Result<GetProductByIdResponse>> _getProductByIdHandler;
        private readonly IQueryHandler<GetActiveProductsQuery, Result<List<GetActiveProductsResponse>>> _getActiveProductsHandler;
        private readonly IQueryHandler<GetLowStockProductsQuery, Result<List<GetLowStockProductsResponse>>> _getLowStockProductsHandler;
        private readonly IQueryHandler<SearchProductsQuery, Result<List<SearchProductsResponse>>> _searchProductsHandler;

        public ProductController(
            ICommandHandler<AddProductCommand, Result<int>> addProductHandler,
            ICommandHandler<UpdateProductCommand, Result<bool>> updateProductHandler,
            ICommandHandler<DeleteProductCommand, Result<bool>> deleteProductHandler,
            ICommandHandler<SoftDeleteProductCommand, Result<bool>> softDeleteProductHandler,
            ICommandHandler<UpdateStockCommand, Result<bool>> updateStockHandler,
            IQueryHandler<GetAllProductsQuery, Result<List<GetAllProductsResponse>>> getAllProductsHandler,
            IQueryHandler<GetProductByIdQuery, Result<GetProductByIdResponse>> getProductByIdHandler,
            IQueryHandler<GetActiveProductsQuery, Result<List<GetActiveProductsResponse>>> getActiveProductsHandler,
            IQueryHandler<GetLowStockProductsQuery, Result<List<GetLowStockProductsResponse>>> getLowStockProductsHandler,
            IQueryHandler<SearchProductsQuery, Result<List<SearchProductsResponse>>> searchProductsHandler)
        {
            _addProductHandler = addProductHandler;
            _updateProductHandler = updateProductHandler;
            _deleteProductHandler = deleteProductHandler;
            _softDeleteProductHandler = softDeleteProductHandler;
            _updateStockHandler = updateStockHandler;
            _getAllProductsHandler = getAllProductsHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _getActiveProductsHandler = getActiveProductsHandler;
            _getLowStockProductsHandler = getLowStockProductsHandler;
            _searchProductsHandler = searchProductsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            var command = new AddProductCommand(request);
            var result = await _addProductHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(id, request);
            var result = await _updateProductHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _deleteProductHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteProduct(int id)
        {
            var command = new SoftDeleteProductCommand(id);
            var result = await _softDeleteProductHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request)
        {
            var command = new UpdateStockCommand(id, request);
            var result = await _updateStockHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _getAllProductsHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _getProductByIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveProducts()
        {
            var query = new GetActiveProductsQuery();
            var result = await _getActiveProductsHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("low-stock")]
        [EnableQuery]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var query = new GetLowStockProductsQuery();
            var result = await _getLowStockProductsHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("search")]
        [EnableQuery]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm)
        {
            var query = new SearchProductsQuery(searchTerm);
            var result = await _searchProductsHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }
    }
} 