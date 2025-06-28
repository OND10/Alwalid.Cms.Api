using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Feedback.Commands.AddFeedback;
using Alwalid.Cms.Api.Features.Feedback.Commands.UpdateFeedback;
using Alwalid.Cms.Api.Features.Feedback.Commands.DeleteFeedback;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetAllFeedbacks;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackById;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetActiveFeedbacks;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetByRating;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetByPosition;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackStats;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Feedback.Dtos;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ODataController
    {
        private readonly ICommandHandler<AddFeedbackCommand, FeedbackResponseDto> _addFeedbackHandler;
        private readonly ICommandHandler<UpdateFeedbackCommand, FeedbackResponseDto> _updateFeedbackHandler;
        private readonly ICommandHandler<DeleteFeedbackCommand, bool> _deleteFeedbackHandler;
        private readonly IQueryHandler<GetAllFeedbacksQuery, IEnumerable<FeedbackResponseDto>> _getAllFeedbacksHandler;
        private readonly IQueryHandler<GetFeedbackByIdQuery, FeedbackResponseDto> _getFeedbackByIdHandler;
        private readonly IQueryHandler<GetActiveFeedbacksQuery, IEnumerable<FeedbackResponseDto>> _getActiveFeedbacksHandler;
        private readonly IQueryHandler<GetByRatingQuery, IEnumerable<FeedbackResponseDto>> _getByRatingHandler;
        private readonly IQueryHandler<GetByPositionQuery, IEnumerable<FeedbackResponseDto>> _getByPositionHandler;
        private readonly IQueryHandler<GetFeedbackStatsQuery, FeedbackStatsDto> _getFeedbackStatsHandler;

        public FeedbackController(
            ICommandHandler<AddFeedbackCommand, FeedbackResponseDto> addFeedbackHandler,
            ICommandHandler<UpdateFeedbackCommand, FeedbackResponseDto> updateFeedbackHandler,
            ICommandHandler<DeleteFeedbackCommand, bool> deleteFeedbackHandler,
            IQueryHandler<GetAllFeedbacksQuery, IEnumerable<FeedbackResponseDto>> getAllFeedbacksHandler,
            IQueryHandler<GetFeedbackByIdQuery, FeedbackResponseDto> getFeedbackByIdHandler,
            IQueryHandler<GetActiveFeedbacksQuery, IEnumerable<FeedbackResponseDto>> getActiveFeedbacksHandler,
            IQueryHandler<GetByRatingQuery, IEnumerable<FeedbackResponseDto>> getByRatingHandler,
            IQueryHandler<GetByPositionQuery, IEnumerable<FeedbackResponseDto>> getByPositionHandler,
            IQueryHandler<GetFeedbackStatsQuery, FeedbackStatsDto> getFeedbackStatsHandler)
        {
            _addFeedbackHandler = addFeedbackHandler;
            _updateFeedbackHandler = updateFeedbackHandler;
            _deleteFeedbackHandler = deleteFeedbackHandler;
            _getAllFeedbacksHandler = getAllFeedbacksHandler;
            _getFeedbackByIdHandler = getFeedbackByIdHandler;
            _getActiveFeedbacksHandler = getActiveFeedbacksHandler;
            _getByRatingHandler = getByRatingHandler;
            _getByPositionHandler = getByPositionHandler;
            _getFeedbackStatsHandler = getFeedbackStatsHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromForm] FeedbackRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddFeedbackCommand
            {
                Request = request
            };
            var result = await _addFeedbackHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateFeedbackCommand
            {
                Id = id,
                Request = request
            };
            var result = await _updateFeedbackHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteFeedbackCommand
            {
                Id = id
            };
            var result = await _deleteFeedbackHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllFeedbacks(CancellationToken cancellationToken)
        {
            var query = new GetAllFeedbacksQuery();
            var result = await _getAllFeedbacksHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(int id, CancellationToken cancellationToken)
        {
            var query = new GetFeedbackByIdQuery
            {
                Id = id
            };
            var result = await _getFeedbackByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpGet("active")]
        [EnableQuery]
        public async Task<IActionResult> GetActiveFeedbacks(CancellationToken cancellationToken)
        {
            var query = new GetActiveFeedbacksQuery();
            var result = await _getActiveFeedbacksHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("rating/{rating}")]
        [EnableQuery]
        public async Task<IActionResult> GetByRating(int rating, CancellationToken cancellationToken)
        {
            var query = new GetByRatingQuery
            {
                Rating = rating
            };
            var result = await _getByRatingHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("position/{position}")]
        [EnableQuery]
        public async Task<IActionResult> GetByPosition(string position, CancellationToken cancellationToken)
        {
            var query = new GetByPositionQuery
            {
                Position = position
            };
            var result = await _getByPositionHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetFeedbackStats(CancellationToken cancellationToken)
        {
            var query = new GetFeedbackStatsQuery();
            var result = await _getFeedbackStatsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }
    }
} 