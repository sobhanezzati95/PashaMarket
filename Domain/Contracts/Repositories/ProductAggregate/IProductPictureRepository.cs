using Domain.Entities.ProductAggregate;
using Framework.Domain;

namespace Domain.Contracts.Repositories.ProductAggregate;
public interface IProductPictureRepository : IRepository<ProductPicture>
{
    Task<ProductPicture> GetWithProductAndCategory(long id,CancellationToken cancellationToken = default);
}