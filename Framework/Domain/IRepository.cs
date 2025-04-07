using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Framework.Domain
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<IQueryable<T>> GetAllAsQueryable();

        Task<T?> GetById(long id);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);

        Task<IQueryable<T>> GetAllWithIncludes(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool asNoTracking = false, bool ignoreQueryFilters = false,
            params Expression<Func<T, object>>[] includeProperties);

        Task<IQueryable<T>> GetAllWithIncludesAndThenInCludes(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool isTracking = false, bool ignoreQueryFilters = false,
            string? includeProperties = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? thenInclude = null);

        Task<T> Add(T entity);
        Task<bool> AddRange(List<T> entities);
        Task<T> Update(T entity);
        Task<bool> UpdateRange(List<T> entities);
        Task<bool> Remove(T entity);
        Task<bool> RemoveRange(List<T> entities);
        Task<bool> Exists(Expression<Func<T, bool>> expression);
    }
}
