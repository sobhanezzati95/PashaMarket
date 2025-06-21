using Framework.Domain;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;
public class BaseRepository<T>
    : IRepository<T> where T : BaseEntity<long>
{
    internal DbSet<T> DbSet;
    public BaseRepository(ApplicationDbContext db)
    {
        DbSet = db.Set<T>();
    }
    public async Task Add(T entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity);
    public async Task AddRange(List<T> entities, CancellationToken cancellationToken = default)
         => await DbSet.AddRangeAsync(entities);
    public async Task<IQueryable<T>> GetAllAsQueryable(CancellationToken cancellationToken = default)
        => await Task.FromResult(DbSet.Where(t => t.IsActive == true).AsQueryable());
    public async Task<IQueryable<T>> GetAllWithIncludesAndThenIncludes(Expression<Func<T, bool>>? predicate = null,
                                                                       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                                       bool isTracking = false, bool ignoreQueryFilters = false,
                                                                       string? includeProperties = null,
                                                                       Func<IQueryable<T>, IIncludableQueryable<T, object>>? thenInclude = null)
    {

        IQueryable<T> query = DbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (includeProperty.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries) != null)
                {
                    query = query.Include(includeProperty);
                }
                query = query.Include(includeProperty);
            }
        }

        if (thenInclude != null)
        {
            query = thenInclude(query);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (isTracking)
        {
            query = query.AsNoTracking();
        }

        return await Task.FromResult((orderBy != null ? orderBy(query) : query));
    }
    public async Task<T?> GetById(long id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync(id);
    public async Task RemoveRange(List<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities.Where(entity => true))
            entity.IsActive = false;

        DbSet.UpdateRange(entities);
    }
    public async Task Update(T entity)
         => DbSet.Update(entity);
    public async Task<bool> Exists(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
         => await DbSet.AnyAsync(expression, cancellationToken);
}