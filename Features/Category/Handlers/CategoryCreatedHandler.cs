using Alwalid.Cms.Api.Abstractions;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Features.Category.Events;
using Alwalid.Cms.Api.Features.Notification.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace Alwalid.Cms.Api.Features.Category.Handlers
{
    public class CategoryCreatedHandler : IDomainEventHandler<CategoryCreatedEvent>
    {
        private readonly ApplicationDbContext _db;
        private readonly INotificationService _notifier;

        public CategoryCreatedHandler(ApplicationDbContext db, INotificationService notifier)
        {
            _db = db;
            _notifier = notifier;
        }

        public async Task HandleAsync(CategoryCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            var market = @event.Category.MarketName;

            var users = await _db.Users
                .Where(u => u.ReceiveCategoryNotifications && u.MarketName == market)
                .ToListAsync(cancellationToken);

            if (users.Count == 0) return;

            var subject = $"New Category in {market}";
            var body =
                $"A new category has been created.\n" +
                $"- English: {@event.Category.EnglishName}\n" +
                $"- Arabic : {@event.Category.ArabicName}\n" +
                $"Created at (UTC): {@event.OccurredOnUtc:O}";

            foreach (var user in users)
            {
                await _notifier.NotifyAsync(user.Email, subject, body, cancellationToken);
            }
        }
    }
}
