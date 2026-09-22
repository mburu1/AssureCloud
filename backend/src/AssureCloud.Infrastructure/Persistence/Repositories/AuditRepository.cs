using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence.Repositories;

public class AuditRepository : IRepository<Audit>
{
    private readonly AssureCloudDbContext _context;

    public AuditRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Audit?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Audits
            .Include(a => a.Findings)
            .Include(a => a.Assignments)
            .Include(a => a.CorrectiveActions)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Audit>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Audits
            .Include(a => a.Findings)
            .Include(a => a.Assignments)
            .Include(a => a.CorrectiveActions)
            .Where(a => !a.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Audit> AddAsync(Audit entity, CancellationToken ct = default)
    {
        await _context.Audits.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Audit entity, CancellationToken ct = default)
    {
        _context.Audits.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Audit entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Audits.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class AuditFindingRepository : IRepository<AuditFinding>
{
    private readonly AssureCloudDbContext _context;

    public AuditFindingRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<AuditFinding?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.AuditFindings
            .Include(f => f.CorrectiveAction)
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<AuditFinding>> ListAsync(CancellationToken ct = default)
    {
        return await _context.AuditFindings
            .Include(f => f.CorrectiveAction)
            .Where(f => !f.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<AuditFinding> AddAsync(AuditFinding entity, CancellationToken ct = default)
    {
        await _context.AuditFindings.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(AuditFinding entity, CancellationToken ct = default)
    {
        _context.AuditFindings.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(AuditFinding entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.AuditFindings.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class CorrectiveActionRepository : IRepository<CorrectiveAction>
{
    private readonly AssureCloudDbContext _context;

    public CorrectiveActionRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<CorrectiveAction?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.CorrectiveActions
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<CorrectiveAction>> ListAsync(CancellationToken ct = default)
    {
        return await _context.CorrectiveActions
            .Where(c => !c.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<CorrectiveAction> AddAsync(CorrectiveAction entity, CancellationToken ct = default)
    {
        await _context.CorrectiveActions.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(CorrectiveAction entity, CancellationToken ct = default)
    {
        _context.CorrectiveActions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(CorrectiveAction entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.CorrectiveActions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class AuditAssignmentRepository : IRepository<AuditAssignment>
{
    private readonly AssureCloudDbContext _context;

    public AuditAssignmentRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<AuditAssignment?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.AuditAssignments
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<AuditAssignment>> ListAsync(CancellationToken ct = default)
    {
        return await _context.AuditAssignments
            .Where(a => !a.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<AuditAssignment> AddAsync(AuditAssignment entity, CancellationToken ct = default)
    {
        await _context.AuditAssignments.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(AuditAssignment entity, CancellationToken ct = default)
    {
        _context.AuditAssignments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(AuditAssignment entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.AuditAssignments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}