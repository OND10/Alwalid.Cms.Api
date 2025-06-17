using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.AddProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.UpdateProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.DeleteProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetAllProductStatistics;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetProductStatisticById;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByProductId;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByDateRange;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetTopSellingProducts;
using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductStatisticController : ODataController
    {
        private readonly ICommandHandler<AddProductStatisticCommand, Result<int>> _addProductStatisticHandler;
        private readonly ICommandHandler<UpdateProductStatisticCommand, Result<bool>> _updateProductStatisticHandler;
        private readonly ICommandHandler<DeleteProductStatisticCommand, Result<bool>> _deleteProductStatisticHandler;
        private readonly IQueryHandler<GetAllProductStatisticsQuery, Result<List<GetAllProductStatisticsResponse>>> _getAllProductStatisticsHandler;
        private readonly IQueryHandler<GetProductStatisticByIdQuery, Result<GetProductStatisticByIdResponse>> _getProductStatisticByIdHandler;
        private readonly IQueryHandler<GetByProductIdQuery, Result<List<GetByProductIdResponse>>> _getByProductIdHandler;
        private readonly IQueryHandler<GetByDateRangeQuery, Result<List<GetByDateRangeResponse>>> _getByDateRangeHandler;
        private readonly IQueryHandler<GetTopSellingProductsQuery, Result<List<GetTopSellingProductsResponse>>> _getTopSellingProductsHandler;

        public ProductStatisticController(
            ICommandHandler<AddProductStatisticCommand, Result<int>> addProductStatisticHandler,
            ICommandHandler<UpdateProductStatisticCommand, Result<bool>> updateProductStatisticHandler,
            ICommandHandler<DeleteProductStatisticCommand, Result<bool>> deleteProductStatisticHandler,
            IQueryHandler<GetAllProductStatisticsQuery, Result<List<GetAllProductStatisticsResponse>>> getAllProductStatisticsHandler,
            IQueryHandler<GetProductStatisticByIdQuery, Result<GetProductStatisticByIdResponse>> getProductStatisticByIdHandler,
            IQueryHandler<GetByProductIdQuery, Result<List<GetByProductIdResponse>>> getByProductIdHandler,
            IQueryHandler<GetByDateRangeQuery, Result<List<GetByDateRangeResponse>>> getByDateRangeHandler,
            IQueryHandler<GetTopSellingProductsQuery, Result<List<GetTopSellingProductsResponse>>> getTopSellingProductsHandler)
        {
            _addProductStatisticHandler = addProductStatisticHandler;
            _updateProductStatisticHandler = updateProductStatisticHandler;
            _deleteProductStatisticHandler = deleteProductStatisticHandler;
            _getAllProductStatisticsHandler = getAllProductStatisticsHandler;
            _getProductStatisticByIdHandler = getProductStatisticByIdHandler;
            _getByProductIdHandler = getByProductIdHandler;
            _getByDateRangeHandler = getByDateRangeHandler;
            _getTopSellingProductsHandler = getTopSellingProductsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductStatistic([FromBody] AddProductStatisticRequest request)
        {
            var command = new AddProductStatisticCommand(request);
            var result = await _addProductStatisticHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductStatistic(int id, [FromBody] UpdateProductStatisticRequest request)
        {
            var command = new UpdateProductStatisticCommand(id, request);
            var result = await _updateProductStatisticHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductStatistic(int id)
        {
            var command = new DeleteProductStatisticCommand(id);
            var result = await _deleteProductStatisticHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllProductStatistics()
        {
            var query = new GetAllProductStatisticsQuery();
            var result = await _getAllProductStatisticsHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductStatisticById(int id)
        {
            var query = new GetProductStatisticByIdQuery(id);
            var result = await _getProductStatisticByIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }

        [HttpGet("product/{productId}")]
        [EnableQuery]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var query = new GetByProductIdQuery(productId);
            var result = await _getByProductIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("date-range")]
        [EnableQuery]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = new GetByDateRangeQuery(startDate, endDate);
            var result = await _getByDateRangeHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }

        [HttpGet("top-selling")]
        [EnableQuery]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int count = 10)
        {
            var query = new GetTopSellingProductsQuery(count);
            var result = await _getTopSellingProductsHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value.AsQueryable());
            
            return BadRequest(result.Error);
        }
    }
} 