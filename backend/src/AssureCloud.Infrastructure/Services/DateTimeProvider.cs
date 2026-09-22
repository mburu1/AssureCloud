using AssureCloud.Application.Abstractions;

namespace AssureCloud.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;

    public DateTime Today => DateTime.Today;

    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    public DateTimeOffset NowOffset => DateTimeOffset.Now;
}