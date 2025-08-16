using Alwalid.Cms.Api.Abstractions;

namespace Alwalid.Cms.Api.Features.Category.Events
{
    public class CategoryCreatedEvent : IDomainEvent
    {
        public Entities.Category Category { get; }
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

        public CategoryCreatedEvent(Entities.Category category)
        {
            Category = category;
        }
    }
}
