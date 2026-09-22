using AssureCloud.Application.Abstractions;
using AssureCloud.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Infrastructure.Persistence;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, ct);
    }

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken ct = default)
    {
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync(ct);
    }

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        entity.ClearDomainEvents();
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
        entity.ClearDomainEvents();
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        await UpdateAsync(entity, ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        await DispatchDomainEventsAsync();
        return await _context.SaveChangesAsync(ct);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var entitiesWithEvents = _context.ChangeTracker.Entries<T>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            entity.ClearDomainEvents();
        }
    }
}
