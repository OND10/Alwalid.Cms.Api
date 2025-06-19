//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Alwalid.Cms.Api.Features.ProductImage.Commands.AddProductImage;
//using Alwalid.Cms.Api.Features.ProductImage.Commands.UpdateProductImage;
//using Alwalid.Cms.Api.Features.ProductImage.Commands.DeleteProductImage;
//using Alwalid.Cms.Api.Features.ProductImage.Queries.GetAllProductImages;
//using Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById;
//using Alwalid.Cms.Api.Features.ProductImage.Queries.GetByProductId;
//using Alwalid.Cms.Api.Abstractions.Messaging;

//namespace Alwalid.Cms.Api.Features.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductImageController : ODataController
//    {
//        private readonly ICommandHandler<AddProductImageCommand, Result<int>> _addProductImageHandler;
//        private readonly ICommandHandler<UpdateProductImageCommand, Result<bool>> _updateProductImageHandler;
//        private readonly ICommandHandler<DeleteProductImageCommand, Result<bool>> _deleteProductImageHandler;
//        private readonly IQueryHandler<GetAllProductImagesQuery, Result<List<GetAllProductImagesResponse>>> _getAllProductImagesHandler;
//        private readonly IQueryHandler<GetProductImageByIdQuery, Result<GetProductImageByIdResponse>> _getProductImageByIdHandler;
//        private readonly IQueryHandler<GetByProductIdQuery, Result<List<GetByProductIdResponse>>> _getByProductIdHandler;

//        public ProductImageController(
//            ICommandHandler<AddProductImageCommand, Result<int>> addProductImageHandler,
//            ICommandHandler<UpdateProductImageCommand, Result<bool>> updateProductImageHandler,
//            ICommandHandler<DeleteProductImageCommand, Result<bool>> deleteProductImageHandler,
//            IQueryHandler<GetAllProductImagesQuery, Result<List<GetAllProductImagesResponse>>> getAllProductImagesHandler,
//            IQueryHandler<GetProductImageByIdQuery, Result<GetProductImageByIdResponse>> getProductImageByIdHandler,
//            IQueryHandler<GetByProductIdQuery, Result<List<GetByProductIdResponse>>> getByProductIdHandler)
//        {
//            _addProductImageHandler = addProductImageHandler;
//            _updateProductImageHandler = updateProductImageHandler;
//            _deleteProductImageHandler = deleteProductImageHandler;
//            _getAllProductImagesHandler = getAllProductImagesHandler;
//            _getProductImageByIdHandler = getProductImageByIdHandler;
//            _getByProductIdHandler = getByProductIdHandler;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddProductImage([FromBody] AddProductImageRequest request)
//        {
//            var command = new AddProductImageCommand(request);
//            var result = await _addProductImageHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateProductImage(int id, [FromBody] UpdateProductImageRequest request)
//        {
//            var command = new UpdateProductImageCommand(id, request);
//            var result = await _updateProductImageHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteProductImage(int id)
//        {
//            var command = new DeleteProductImageCommand(id);
//            var result = await _deleteProductImageHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpGet]
//        [EnableQuery]
//        public async Task<IActionResult> GetAllProductImages()
//        {
//            var query = new GetAllProductImagesQuery();
//            var result = await _getAllProductImagesHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value.AsQueryable());
            
//            return BadRequest(result.Error);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetProductImageById(int id)
//        {
//            var query = new GetProductImageByIdQuery(id);
//            var result = await _getProductImageByIdHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return NotFound(result.Error);
//        }

//        [HttpGet("product/{productId}")]
//        [EnableQuery]
//        public async Task<IActionResult> GetByProductId(int productId)
//        {
//            var query = new GetByProductIdQuery(productId);
//            var result = await _getByProductIdHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value.AsQueryable());
            
//            return BadRequest(result.Error);
//        }
//    }
//} 