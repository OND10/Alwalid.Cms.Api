using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Feedback.Dtos;

namespace Alwalid.Cms.Api.Features.Feedback.Queries.GetByPosition
{
    public class GetByPositionQuery : IQuery<IEnumerable<FeedbackResponseDto>>
    {
        public string Position { get; set; } = string.Empty;
    }
} 