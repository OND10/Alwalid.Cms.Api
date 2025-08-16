namespace Alwalid.Cms.Api.Features.Notification.Services
{
    public interface INotificationService
    {
        Task NotifyAsync(string email, string subject, string body, CancellationToken ct = default);

    }
}
