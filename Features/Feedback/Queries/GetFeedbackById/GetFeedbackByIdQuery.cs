using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackById
{
    public class GetFeedbackByIdQuery : IQuery<FeedbackResponseDto>
    {
        public int Id { get; set; }
    }
} 