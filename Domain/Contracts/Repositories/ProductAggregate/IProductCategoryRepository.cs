using Domain.Entities.ProductAggregate;
using Framework.Domain;

namespace Domain.Contracts.Repositories.ProductAggregate
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<string> GetSlugById(long id);
    }
}
