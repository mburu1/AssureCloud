using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence.Repositories;

public class CertificationRepository : IRepository<Certification>
{
    private readonly AssureCloudDbContext _context;

    public CertificationRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Certification?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Certifications
            .Include(c => c.Decisions)
            .Include(c => c.Scopes)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Certification>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Certifications
            .Include(c => c.Decisions)
            .Include(c => c.Scopes)
            .Where(c => !c.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Certification> AddAsync(Certification entity, CancellationToken ct = default)
    {
        await _context.Certifications.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Certification entity, CancellationToken ct = default)
    {
        _context.Certifications.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Certification entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Certifications.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class CertificationDecisionRepository : IRepository<CertificationDecision>
{
    private readonly AssureCloudDbContext _context;

    public CertificationDecisionRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<CertificationDecision?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.CertificationDecisions
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<CertificationDecision>> ListAsync(CancellationToken ct = default)
    {
        return await _context.CertificationDecisions
            .Where(d => !d.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<CertificationDecision> AddAsync(CertificationDecision entity, CancellationToken ct = default)
    {
        await _context.CertificationDecisions.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(CertificationDecision entity, CancellationToken ct = default)
    {
        _context.CertificationDecisions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(CertificationDecision entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.CertificationDecisions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class CertificationScopeRepository : IRepository<CertificationScope>
{
    private readonly AssureCloudDbContext _context;

    public CertificationScopeRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<CertificationScope?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.CertificationScopes
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<CertificationScope>> ListAsync(CancellationToken ct = default)
    {
        return await _context.CertificationScopes
            .Where(s => !s.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<CertificationScope> AddAsync(CertificationScope entity, CancellationToken ct = default)
    {
        await _context.CertificationScopes.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(CertificationScope entity, CancellationToken ct = default)
    {
        _context.CertificationScopes.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(CertificationScope entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.CertificationScopes.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}