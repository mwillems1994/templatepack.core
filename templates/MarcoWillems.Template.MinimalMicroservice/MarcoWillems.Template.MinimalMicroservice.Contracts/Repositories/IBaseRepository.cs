using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Repositories;

public interface IBaseRepository<TSource> where TSource : class
{
    Task AddAsync(TSource item, CancellationToken cancellationToken = default);
    Task AddRangeAsync(TSource[] items, CancellationToken cancellationToken = default);
    Task<TResult[]> GetAsync<TResult>(Expression<Func<TSource, TResult>> selector, Expression<Func<TSource, bool>>? filter = null, CancellationToken cancellationToken = default);
    Task<TResult?> FindAsync<TResult>(Guid id, Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(TSource item, bool hardDelete = false, CancellationToken cancellationToken = default);
}