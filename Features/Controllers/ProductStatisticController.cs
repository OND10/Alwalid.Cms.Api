using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.AddProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.UpdateProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.DeleteProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.IncrementViewCount;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetAllProductStatistics;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetProductStatisticById;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByProductId;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByDateRange;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetTopSellingProducts;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetMostViewedProducts;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductStatisticController : ODataController
    {
        private readonly ICommandHandler<AddProductStatisticCommand, ProductStatisticResponseDto> _addProductStatisticHandler;
        private readonly ICommandHandler<UpdateProductStatisticCommand, ProductStatisticResponseDto> _updateProductStatisticHandler;
        private readonly ICommandHandler<DeleteProductStatisticCommand, bool> _deleteProductStatisticHandler;
        private readonly ICommandHandler<IncrementViewCountCommand, ProductStatisticResponseDto> _incrementViewCountHandler;
        private readonly IQueryHandler<GetAllProductStatisticsQuery, IEnumerable<ProductStatisticResponseDto>> _getAllProductStatisticsHandler;
        private readonly IQueryHandler<GetProductStatisticByIdQuery, ProductStatisticResponseDto> _getProductStatisticByIdHandler;
        private readonly IQueryHandler<GetByProductIdForStatisticQuery, IEnumerable<ProductStatisticResponseDto>> _getByProductIdHandler;
        private readonly IQueryHandler<GetByDateRangeQuery, IEnumerable<ProductStatisticResponseDto>> _getByDateRangeHandler;
        private readonly IQueryHandler<GetTopSellingProductsQuery, IEnumerable<ProductStatisticResponseDto>> _getTopSellingProductsHandler;
        private readonly IQueryHandler<GetMostViewedProductsQuery, IEnumerable<ProductStatisticResponseDto>> _getMostViewedProductsHandler;

        public ProductStatisticController(
            ICommandHandler<AddProductStatisticCommand, ProductStatisticResponseDto> addProductStatisticHandler,
            ICommandHandler<UpdateProductStatisticCommand, ProductStatisticResponseDto> updateProductStatisticHandler,
            ICommandHandler<DeleteProductStatisticCommand, bool> deleteProductStatisticHandler,
            ICommandHandler<IncrementViewCountCommand, ProductStatisticResponseDto> incrementViewCountHandler,
            IQueryHandler<GetAllProductStatisticsQuery, IEnumerable<ProductStatisticResponseDto>> getAllProductStatisticsHandler,
            IQueryHandler<GetProductStatisticByIdQuery, ProductStatisticResponseDto> getProductStatisticByIdHandler,
            IQueryHandler<GetByProductIdForStatisticQuery, IEnumerable<ProductStatisticResponseDto>> getByProductIdHandler,
            IQueryHandler<GetByDateRangeQuery, IEnumerable<ProductStatisticResponseDto>> getByDateRangeHandler,
            IQueryHandler<GetTopSellingProductsQuery, IEnumerable<ProductStatisticResponseDto>> getTopSellingProductsHandler,
            IQueryHandler<GetMostViewedProductsQuery, IEnumerable<ProductStatisticResponseDto>> getMostViewedProductsHandler)
        {
            _addProductStatisticHandler = addProductStatisticHandler;
            _updateProductStatisticHandler = updateProductStatisticHandler;
            _deleteProductStatisticHandler = deleteProductStatisticHandler;
            _incrementViewCountHandler = incrementViewCountHandler;
            _getAllProductStatisticsHandler = getAllProductStatisticsHandler;
            _getProductStatisticByIdHandler = getProductStatisticByIdHandler;
            _getByProductIdHandler = getByProductIdHandler;
            _getByDateRangeHandler = getByDateRangeHandler;
            _getTopSellingProductsHandler = getTopSellingProductsHandler;
            _getMostViewedProductsHandler = getMostViewedProductsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductStatistic([FromBody] ProductStatisticRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddProductStatisticCommand
            {
                Request = request,
            };
            var result = await _addProductStatisticHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductStatistic(int id, [FromBody] ProductStatisticRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateProductStatisticCommand
            {
                Request = request,
                Id = id
            };
            var result = await _updateProductStatisticHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductStatistic(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductStatisticCommand
            {
                Id = id
            };
            var result = await _deleteProductStatisticHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllProductStatistics(CancellationToken cancellationToken)
        {
            var query = new GetAllProductStatisticsQuery();
            var result = await _getAllProductStatisticsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductStatisticById(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductStatisticByIdQuery
            {
                Id = id
            };
            var result = await _getProductStatisticByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("product/{productId}")]
        [EnableQuery]
        public async Task<IActionResult> GetByProductId(int productId, CancellationToken cancellationToken)
        {
            var query = new GetByProductIdForStatisticQuery
            {
                ProductId = productId
            };
            var result = await _getByProductIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("date-range")]
        [EnableQuery]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            var query = new GetByDateRangeQuery
            {
                EndDate = endDate,
                StartDate = startDate,
            };
            var result = await _getByDateRangeHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("top-selling")]
        [EnableQuery]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] TopSellingProductDto request, CancellationToken cancellationToken)
        {
            var query = new GetTopSellingProductsQuery
            {
                Request = request
            };
            var result = await _getTopSellingProductsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpPost("increment-view-count")]
        public async Task<IActionResult> IncrementViewCount([FromBody] int productId, CancellationToken cancellationToken)
        {
            var command = new IncrementViewCountCommand
            {
                ProductId = productId
            };
            var result = await _incrementViewCountHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet("most-viewed")]
        [EnableQuery]
        public async Task<IActionResult> GetMostViewedProducts([FromQuery] MostViewedProductDto request, CancellationToken cancellationToken)
        {
            var query = new GetMostViewedProductsQuery
            {
                Request = request
            };
            var result = await _getMostViewedProductsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }
    }
}