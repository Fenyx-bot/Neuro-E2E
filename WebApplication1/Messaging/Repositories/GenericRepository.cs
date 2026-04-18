using System.Linq.Expressions;
using Messaging.DAL;
using Messaging.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Messaging.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<T?> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        => await _dbSet.AnyAsync(filter);

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters().AsNoTracking();

        return query.FirstOrDefaultAsync(filter);
    }

    public Task<List<T>> WhereAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet.IgnoreQueryFilters().AsNoTracking();

        return query.Where(filter).ToListAsync();
    }

    public async Task LoadCollectionAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> collection)
        where TProperty : class
    {
        await context.Entry(entity).Collection(collection).LoadAsync();
    }

    public async Task LoadReferenceAsync<TProperty>(T entity, Expression<Func<T, TProperty?>> reference)
        where TProperty : class
    {
        await context.Entry(entity).Reference(reference).LoadAsync();
    }

    public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
    {
        return _dbSet.Include(navigationPropertyPath);
    }

    public async Task<IDbContextTransaction> BeginTransaction()
    {
        return await context.Database.BeginTransactionAsync();
    }
}
