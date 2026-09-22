using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssureCloud.Application.Abstractions;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, CancellationToken ct = default);
    Task SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = false, CancellationToken ct = default);
    Task SendNotificationAsync(string userId, string title, string message, CancellationToken ct = default);
}
