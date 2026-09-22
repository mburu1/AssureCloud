using System;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Application.Abstractions;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> CanSaveChangesAsync(CancellationToken cancellationToken = default);
    void BeginTransaction();
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
