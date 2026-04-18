using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Messaging.Interfaces.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task<List<T>> WhereAsync(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Loads a collection property for an entity.
    /// </summary>
    Task LoadCollectionAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> collection) where TProperty : class;

    /// <summary>
    /// Loads a reference (single navigation property) for an entity.
    /// </summary>
    Task LoadReferenceAsync<TProperty>(T entity, Expression<Func<T, TProperty?>> reference) where TProperty : class;

    /// <summary>
    /// Allows including related data in a query.
    /// </summary>
    IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);

    Task<IDbContextTransaction> BeginTransaction();
}
