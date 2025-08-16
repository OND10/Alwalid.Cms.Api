namespace Alwalid.Cms.Api.Features.Notification.Services
{
    public class ConsoleNotificationService : INotificationService
    {
        public Task NotifyAsync(string email, string subject, string body, CancellationToken ct = default)
        {
            Console.WriteLine($"[Notify] -> {email}\nSubject: {subject}\n{body}\n");
            return Task.CompletedTask;
        }
    }
}
