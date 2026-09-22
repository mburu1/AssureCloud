using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssureCloud.Infrastructure.Persistence.Repositories;

public class OrganizationRepository : IRepository<Organization>
{
    private readonly AssureCloudDbContext _context;

    public OrganizationRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Organizations
            .Include(o => o.Locations)
            .Include(o => o.Suppliers)
            .Include(o => o.Users)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Organization>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Organizations
            .Include(o => o.Locations)
            .Include(o => o.Suppliers)
            .Include(o => o.Users)
            .Where(o => !o.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Organization> AddAsync(Organization entity, CancellationToken ct = default)
    {
        await _context.Organizations.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Organization entity, CancellationToken ct = default)
    {
        _context.Organizations.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Organization entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Organizations.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class OrganizationLocationRepository : IRepository<OrganizationLocation>
{
    private readonly AssureCloudDbContext _context;

    public OrganizationLocationRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<OrganizationLocation?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.OrganizationLocations
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<OrganizationLocation>> ListAsync(CancellationToken ct = default)
    {
        return await _context.OrganizationLocations
            .Where(l => !l.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<OrganizationLocation> AddAsync(OrganizationLocation entity, CancellationToken ct = default)
    {
        await _context.OrganizationLocations.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(OrganizationLocation entity, CancellationToken ct = default)
    {
        _context.OrganizationLocations.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(OrganizationLocation entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.OrganizationLocations.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class SupplierRepository : IRepository<Supplier>
{
    private readonly AssureCloudDbContext _context;

    public SupplierRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<Supplier>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Suppliers
            .Where(s => !s.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<Supplier> AddAsync(Supplier entity, CancellationToken ct = default)
    {
        await _context.Suppliers.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(Supplier entity, CancellationToken ct = default)
    {
        _context.Suppliers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Supplier entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Suppliers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}

public class UserRepository : IRepository<User>
{
    private readonly AssureCloudDbContext _context;

    public UserRepository(AssureCloudDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, ct);
    }

    public async Task<IReadOnlyList<User>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Users
            .Where(u => !u.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task<User> AddAsync(User entity, CancellationToken ct = default)
    {
        await _context.Users.AddAsync(entity, ct);
        return entity;
    }

    public async Task UpdateAsync(User entity, CancellationToken ct = default)
    {
        _context.Users.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(User entity, CancellationToken ct = default)
    {
        entity.MarkAsDeleted();
        _context.Users.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}