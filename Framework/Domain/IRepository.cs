﻿using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Framework.Domain;
public interface IRepository<T> where T : class
{
    Task<IQueryable<T>> GetAllAsQueryable(CancellationToken cancellationToken = default);
    Task<T> GetById(long id, CancellationToken cancellationToken = default);
    Task Add(T entity, CancellationToken cancellationToken = default);
    Task AddRange(List<T> entities, CancellationToken cancellationToken = default);
    Task Update(T entity);
    Task RemoveRange(List<T> entities, CancellationToken cancellationToken = default);
    Task<bool> Exists(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<IQueryable<T>> GetAllWithIncludesAndThenIncludes(Expression<Func<T, bool>>? predicate = null,
                                                      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                      bool isTracking = false, bool ignoreQueryFilters = false,
                                                      string? includeProperties = null,
                                                      Func<IQueryable<T>, IIncludableQueryable<T, object>>? thenInclude = null);
}