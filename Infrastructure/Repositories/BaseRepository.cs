using Framework.Domain;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;
public class BaseRepository<T> : IRepository<T> where T : BaseEntity<long>
{
    internal DbSet<T> DbSet;

    public BaseRepository(ApplicationDbContext db)
    {
        DbSet = db.Set<T>();
    }

    public async Task<T> Add(T entity)
    {
        entity.CreateDateTime = DateTime.UtcNow;
        await DbSet.AddAsync(entity);
        return entity;
    }

    public async Task<bool> AddRange(List<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
        return await Task.FromResult(true);
    }

    public async Task<List<T>> GetAll()
    {
        return await DbSet.Where(t => t.IsRemoved == false && t.IsActive == true).ToListAsync();
    }
    public async Task<IQueryable<T>> GetAllAsQueryable()
    {
        return await Task.FromResult(DbSet.Where(t => t.IsRemoved == false && t.IsActive == true).AsQueryable());

    }

    public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).Where(t => t.IsRemoved == false && t.IsActive == true).ToListAsync();
    }
    public async Task<IQueryable<T>> GetAllWithIncludesAndThenInCludes(Expression<Func<T, bool>>? predicate = null,
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

    public async Task<IQueryable<TResult>> GetAllWithIncludesAndThenInCludes<TResult>(
     Expression<Func<T, bool>>? predicate = null,
     Func<IQueryable<TResult>, IOrderedQueryable<TResult>>? orderBy = null,
     bool isNoTracking = false,
     bool ignoreQueryFilters = false,
     string? includeProperties = null,
     Func<IQueryable<T>, IIncludableQueryable<T, object>>? thenInclude = null,
     Func<IQueryable<T>, IQueryable<TResult>>? select = null)
    {
        IQueryable<T> query = DbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (includeProperty.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries) != null)
                {
                    query = query.Include(includeProperty);
                }
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

        if (isNoTracking)
        {
            query = query.AsNoTracking();
        }

        IQueryable<TResult> projectedQuery = select != null ? select(query) : query.Cast<TResult>();

        return await Task.FromResult(orderBy != null ? orderBy(projectedQuery) : projectedQuery);
    }



    public async Task<IQueryable<T>> GetAllWithIncludes(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool asNoTracking = false, bool ignoreQueryFilters = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = DbSet;

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }
        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }


        return await Task.FromResult((orderBy != null ? orderBy(query) : query));
    }

    public async Task<T?> GetById(long id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<bool> Remove(T entity)
    {
        entity.IsActive = false;
        entity.IsRemoved = true;
        entity.ModifyDateTime = DateTime.UtcNow;
        DbSet.Update(entity);
        return await Task.FromResult(true);
    }

    public async Task<bool> RemoveRange(List<T> entities)
    {
        var baseEntities = entities.ToList();
        foreach (var entity in baseEntities.Where(entity => true))
        {
            entity.IsActive = false;
            entity.IsRemoved = true;
            entity.ModifyDateTime = DateTime.UtcNow;
        }
        DbSet.UpdateRange(baseEntities);
        return await Task.FromResult(true);
    }

    public async Task<T> Update(T entity)
    {
        entity.ModifyDateTime = DateTime.UtcNow;
        entity.IsModified = true;
        DbSet.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task<bool> UpdateRange(List<T> entities)
    {
        DbSet.UpdateRange(entities);
        return await Task.FromResult(true);
    }

    public async Task<bool> Exists(Expression<Func<T, bool>> expression)
    {
        return await DbSet.AnyAsync(expression);
    }
}
