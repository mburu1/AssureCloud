using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence.Repositories;

public class ProgramRepository : IRepository<Program>
{
    private readonly AssureCloudDbContext _context;

    public ProgramRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Program?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Programs
            .Include(p => p.Standards)
                .ThenInclude(s => s.Requirements)
                    .ThenInclude(r => r.Criteria)
                        .ThenInclude(c => c.Controls)
            .Include(p => p.Assessments)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Program>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Programs
            .Include(p => p.Standards)
            .Include(p => p.Assessments)
            .Where(p => !p.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Program> AddAsync(Program entity, CancellationToken ct = default)
    {
        await _context.Programs.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Program entity, CancellationToken ct = default)
    {
        _context.Programs.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Program entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Programs.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class StandardRepository : IRepository<Standard>
{
    private readonly AssureCloudDbContext _context;

    public StandardRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Standard?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Standards
            .Include(s => s.Requirements)
                .ThenInclude(r => r.Criteria)
                    .ThenInclude(c => c.Controls)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Standard>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Standards
            .Include(s => s.Requirements)
            .Where(s => !s.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Standard> AddAsync(Standard entity, CancellationToken ct = default)
    {
        await _context.Standards.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Standard entity, CancellationToken ct = default)
    {
        _context.Standards.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Standard entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Standards.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class RequirementRepository : IRepository<Requirement>
{
    private readonly AssureCloudDbContext _context;

    public RequirementRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Requirement?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Requirements
            .Include(r => r.Criteria)
                .ThenInclude(c => c.Controls)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Requirement>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Requirements
            .Include(r => r.Criteria)
            .Where(r => !r.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Requirement> AddAsync(Requirement entity, CancellationToken ct = default)
    {
        await _context.Requirements.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Requirement entity, CancellationToken ct = default)
    {
        _context.Requirements.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Requirement entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Requirements.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class CriterionRepository : IRepository<Criterion>
{
    private readonly AssureCloudDbContext _context;

    public CriterionRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Criterion?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Criteria
            .Include(c => c.Controls)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Criterion>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Criteria
            .Include(c => c.Controls)
            .Where(c => !c.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Criterion> AddAsync(Criterion entity, CancellationToken ct = default)
    {
        await _context.Criteria.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Criterion entity, CancellationToken ct = default)
    {
        _context.Criteria.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Criterion entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Criteria.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class ControlRepository : IRepository<Control>
{
    private readonly AssureCloudDbContext _context;

    public ControlRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Control?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Controls
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Control>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Controls
            .Where(c => !c.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Control> AddAsync(Control entity, CancellationToken ct = default)
    {
        await _context.Controls.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Control entity, CancellationToken ct = default)
    {
        _context.Controls.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Control entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Controls.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}