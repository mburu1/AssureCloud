using System;

namespace AssureCloud.Infrastructure;

public class DateTimeService : Application.Abstractions.IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
