using Domain.Entities.ProductAggregate;
using Framework.Domain;

namespace Domain.Contracts.Repositories.ProductAggregate;
public interface IProductRepository : IRepository<Product>
{
    Task<Product> GetProductWithCategory(long id, CancellationToken cancellationToken = default);
}