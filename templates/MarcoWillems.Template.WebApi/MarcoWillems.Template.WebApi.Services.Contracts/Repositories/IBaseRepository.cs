using System.Linq.Expressions;

namespace MarcoWillems.Template.WebApi.Services.Contracts.Repositories;
public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T item, CancellationToken cancellationToken = default);
    Task AddRangeAsync(T[] items, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(T item, bool hardDelete = false, CancellationToken cancellationToken = default);
    Task<TResult[]> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);
}