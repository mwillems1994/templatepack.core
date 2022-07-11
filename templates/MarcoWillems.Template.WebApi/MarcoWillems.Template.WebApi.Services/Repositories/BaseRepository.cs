using Microsoft.EntityFrameworkCore;
using MarcoWillems.Template.WebApi.Database.Context;
using MarcoWillems.Template.WebApi.Database.Entities.Common;
using MarcoWillems.Template.WebApi.Services.Contracts.Repositories;
using System.Linq.Expressions;

namespace MarcoWillems.Template.WebApi.Services.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    internal readonly CustomDbContext _context;
    internal readonly DbSet<T> _dbSet;

    public BaseRepository(CustomDbContext dbContext)
    {
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T item, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(item, cancellationToken);
    }

    public async Task AddRangeAsync(T[] items, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(items, cancellationToken);
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

    public async Task<TResult[]> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
            .Select(selector)
            .ToArrayAsync(cancellationToken);

        return result;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

