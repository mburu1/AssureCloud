using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Application.Services;

public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
    Task<Func<Task, Task>?> DequeueAsync(CancellationToken cancellationToken);
    int Count { get; }
    bool IsCompleted { get; }
}
