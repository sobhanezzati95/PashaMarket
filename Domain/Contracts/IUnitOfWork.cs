using Domain.Contracts.Repositories.ProductAggregate;

namespace Domain;
public interface IUnitOfWork : IDisposable
{

    #region ProductAggRepo

    IProductRepository ProductRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    IProductPictureRepository ProductPictureRepository { get; }

    #endregion

    Task<int> CommitAsync();
}
