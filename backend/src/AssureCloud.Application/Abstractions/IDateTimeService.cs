using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Application.Abstractions;

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
