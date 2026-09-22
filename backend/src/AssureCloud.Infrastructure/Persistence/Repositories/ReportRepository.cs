using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence.Repositories;

public class ReportRepository : IRepository<Report>
{
    private readonly AssureCloudDbContext _context;

    public ReportRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Report?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Reports
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Report>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Reports
            .Where(r => !r.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Report> AddAsync(Report entity, CancellationToken ct = default)
    {
        await _context.Reports.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Report entity, CancellationToken ct = default)
    {
        _context.Reports.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Report entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Reports.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}