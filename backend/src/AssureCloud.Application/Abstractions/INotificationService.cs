using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Application.Abstractions;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendEmailAsync(string to, string subject, string body, IEnumerable<string> cc, IEnumerable<string> bcc, CancellationToken cancellationToken = default);
    Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    Task SendPushNotificationAsync(string userId, string title, string body, IDictionary<string, string>? data = null, CancellationToken cancellationToken = default);
    Task SendInAppNotificationAsync(string userId, string title, string message, string? link = null, CancellationToken cancellationToken = default);
}