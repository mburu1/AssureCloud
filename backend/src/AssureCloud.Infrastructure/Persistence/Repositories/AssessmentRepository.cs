using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence.Repositories;

public class AssessmentRepository : IRepository<Assessment>
{
    private readonly AssureCloudDbContext _context;

    public AssessmentRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Assessment?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Assessments
            .Include(a => a.Responses)
            .Include(a => a.Evidence)
            .Include(a => a.Findings)
            .Include(a => a.Assignments)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Assessment>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Assessments
            .Include(a => a.Responses)
            .Include(a => a.Evidence)
            .Include(a => a.Findings)
            .Include(a => a.Assignments)
            .Where(a => !a.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Assessment> AddAsync(Assessment entity, CancellationToken ct = default)
    {
        await _context.Assessments.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Assessment entity, CancellationToken ct = default)
    {
        _context.Assessments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Assessment entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Assessments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class AssessmentResponseRepository : IRepository<AssessmentResponse>
{
    private readonly AssureCloudDbContext _context;

    public AssessmentResponseRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<AssessmentResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.AssessmentResponses
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<AssessmentResponse>> ListAsync(CancellationToken ct = default)
    {
        return await _context.AssessmentResponses
            .Where(r => !r.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<AssessmentResponse> AddAsync(AssessmentResponse entity, CancellationToken ct = default)
    {
        await _context.AssessmentResponses.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(AssessmentResponse entity, CancellationToken ct = default)
    {
        _context.AssessmentResponses.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(AssessmentResponse entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.AssessmentResponses.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class EvidenceRepository : IRepository<Evidence>
{
    private readonly AssureCloudDbContext _context;

    public EvidenceRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Evidence?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Evidence
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Evidence>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Evidence
            .Where(e => !e.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Evidence> AddAsync(Evidence entity, CancellationToken ct = default)
    {
        await _context.Evidence.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Evidence entity, CancellationToken ct = default)
    {
        _context.Evidence.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Evidence entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Evidence.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class FindingRepository : IRepository<Finding>
{
    private readonly AssureCloudDbContext _context;

    public FindingRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Finding?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Findings
            .Include(f => f.CorrectiveAction)
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Finding>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Findings
            .Include(f => f.CorrectiveAction)
            .Where(f => !f.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Finding> AddAsync(Finding entity, CancellationToken ct = default)
    {
        await _context.Findings.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Finding entity, CancellationToken ct = default)
    {
        _context.Findings.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Finding entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Findings.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class AssessmentAssignmentRepository : IRepository<AssessmentAssignment>
{
    private readonly AssureCloudDbContext _context;

    public AssessmentAssignmentRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<AssessmentAssignment?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.AssessmentAssignments
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<AssessmentAssignment>> ListAsync(CancellationToken ct = default)
    {
        return await _context.AssessmentAssignments
            .Where(a => !a.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<AssessmentAssignment> AddAsync(AssessmentAssignment entity, CancellationToken ct = default)
    {
        await _context.AssessmentAssignments.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(AssessmentAssignment entity, CancellationToken ct = default)
    {
        _context.AssessmentAssignments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(AssessmentAssignment entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.AssessmentAssignments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}