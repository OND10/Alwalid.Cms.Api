using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.ProductImage.Commands.AddProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Commands.UpdateProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Commands.DeleteProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetAllProductImages;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetByProductId;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ODataController
    {
        private readonly ICommandHandler<AddProductImageCommand, ProductImageResponseDto> _addProductImageHandler;
        private readonly ICommandHandler<UpdateProductImageCommand, ProductImageResponseDto> _updateProductImageHandler;
        private readonly ICommandHandler<DeleteProductImageCommand, bool> _deleteProductImageHandler;
        private readonly IQueryHandler<GetAllProductImagesQuery, IEnumerable<ProductImageResponseDto>> _getAllProductImagesHandler;
        private readonly IQueryHandler<GetProductImageByIdQuery, ProductImageResponseDto> _getProductImageByIdHandler;
        private readonly IQueryHandler<GetByProductIdQuery, IEnumerable<ProductImageResponseDto>> _getByProductIdHandler;

        public ProductImageController(
            ICommandHandler<AddProductImageCommand, ProductImageResponseDto> addProductImageHandler,
            ICommandHandler<UpdateProductImageCommand, ProductImageResponseDto> updateProductImageHandler,
            ICommandHandler<DeleteProductImageCommand,bool> deleteProductImageHandler,
            IQueryHandler<GetAllProductImagesQuery, IEnumerable<ProductImageResponseDto>> getAllProductImagesHandler,
            IQueryHandler<GetProductImageByIdQuery, ProductImageResponseDto> getProductImageByIdHandler,
            IQueryHandler<GetByProductIdQuery, IEnumerable<ProductImageResponseDto>> getByProductIdHandler)
        {
            _addProductImageHandler = addProductImageHandler;
            _updateProductImageHandler = updateProductImageHandler;
            _deleteProductImageHandler = deleteProductImageHandler;
            _getAllProductImagesHandler = getAllProductImagesHandler;
            _getProductImageByIdHandler = getProductImageByIdHandler;
            _getByProductIdHandler = getByProductIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductImage([FromForm] ProductImageRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddProductImageCommand
            {
                Image = request.image,
                ProductId = request.ProductId
            };
            var result = await _addProductImageHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(int id, [FromForm] ProductImageRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductImageCommand
            {
                Request = request,
                Id = id,
                
            };
            var result = await _updateProductImageHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductImageCommand
            {
                Id = id
            };
            var result = await _deleteProductImageHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllProductImages(CancellationToken cancellationToken)
        {
            var query = new GetAllProductImagesQuery();
            var result = await _getAllProductImagesHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImageById(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductImageByIdQuery
            {
                Id = id
            };
            var result = await _getProductImageByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("product/{productId}")]
        [EnableQuery]
        public async Task<IActionResult> GetByProductId(int productId, CancellationToken cancellationToken)
        {
            var query = new GetByProductIdQuery
            {
                ProductId = productId
            };
            var result = await _getByProductIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }
    }
}