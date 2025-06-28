using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetByRating
{
    public class GetByRatingQuery : IQuery<IEnumerable<FeedbackResponseDto>>
    {
        public int Rating { get; set; }
    }
} 