using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Entities.Common;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Repositories;
using MarcoWillems.Template.MinimalMicroservice.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MarcoWillems.Template.MinimalMicroservice.Services.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    internal readonly CustomDbContext _context;
    internal readonly DbSet<T> _dbSet;

    public BaseRepository(CustomDbContext dbContext)
    {
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _context.Set<T>();
    }

    private readonly Expression<Func<T, bool>> NoFilter = _ => true;

    public async Task AddAsync(T item, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(item, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(T[] items, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(items, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<TResult[]> GetAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
            .Where(filter ?? NoFilter)
            .Select(selector)
            .ToArrayAsync(cancellationToken);

        return result;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T item, bool hardDelete = false, CancellationToken cancellationToken = default)
    {
        if (item is EntityLog itemLog && !hardDelete)
        {
            itemLog.Deleted = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return;
        }

        _dbSet.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}