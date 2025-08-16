using System;

namespace Alwalid.Cms.Api.Abstractions
{
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}
