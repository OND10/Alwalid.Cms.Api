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
using Alwalid.Cms.Api.Features.Product.Dtos;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ODataController
    {
        private readonly ICommandHandler<AddProductCommand, ProductResponseDto> _addProductHandler;
        private readonly ICommandHandler<UpdateProductCommand, ProductResponseDto> _updateProductHandler;
        private readonly ICommandHandler<DeleteProductCommand, bool> _deleteProductHandler;
        private readonly ICommandHandler<SoftDeleteProductCommand, bool> _softDeleteProductHandler;
        private readonly ICommandHandler<UpdateStockCommand, bool> _updateStockHandler;
        private readonly IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>> _getAllProductsHandler;
        private readonly IQueryHandler<GetProductByIdQuery, ProductResponseDto> _getProductByIdHandler;
        private readonly IQueryHandler<GetActiveProductsQuery, IEnumerable<ProductResponseDto>> _getActiveProductsHandler;
        private readonly IQueryHandler<GetLowStockProductsQuery, IEnumerable<ProductResponseDto>> _getLowStockProductsHandler;
        private readonly IQueryHandler<SearchProductsQuery, IEnumerable<ProductResponseDto>> _searchProductsHandler;

        public ProductController(
            ICommandHandler<AddProductCommand, ProductResponseDto> addProductHandler,
            ICommandHandler<UpdateProductCommand, ProductResponseDto> updateProductHandler,
            ICommandHandler<DeleteProductCommand, bool> deleteProductHandler,
            ICommandHandler<SoftDeleteProductCommand, bool> softDeleteProductHandler,
            ICommandHandler<UpdateStockCommand, bool> updateStockHandler,
            IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>> getAllProductsHandler,
            IQueryHandler<GetProductByIdQuery, ProductResponseDto> getProductByIdHandler,
            IQueryHandler<GetActiveProductsQuery, IEnumerable<ProductResponseDto>> getActiveProductsHandler,
            IQueryHandler<GetLowStockProductsQuery, IEnumerable<ProductResponseDto>> getLowStockProductsHandler,
            IQueryHandler<SearchProductsQuery, IEnumerable<ProductResponseDto>> searchProductsHandler)
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
        public async Task<IActionResult> AddProduct([FromForm] ProductRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddProductCommand
            {
                Request = request,
                
            };
            var result = await _addProductHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Request = request
            };
            var result = await _updateProductHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand
            {
                Id = id
            };
            var result = await _deleteProductHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteProduct(int id, CancellationToken cancellationToken)
        {
            var command = new SoftDeleteProductCommand
            {
                Id= id
            };
            var result = await _softDeleteProductHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] StockRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateStockCommand
            {
                Id = id,
                Request = request
            };
            var result = await _updateStockHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
        {
            var query = new GetAllProductsQuery();
            var result = await _getAllProductsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery
            {
                Id = id,
            };
            var result = await _getProductByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveProducts(CancellationToken cancellationToken)
        {
            var query = new GetActiveProductsQuery();
            var result = await _getActiveProductsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("low-stock")]
        [EnableQuery]
        public async Task<IActionResult> GetLowStockProducts(CancellationToken cancellationToken)
        {
            var query = new GetLowStockProductsQuery();
            var result = await _getLowStockProductsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("search")]
        [EnableQuery]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm, CancellationToken cancellationToken)
        {
            var query = new SearchProductsQuery
            {
                SearchTerm = searchTerm
            };
            var result = await _searchProductsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }
    }
}