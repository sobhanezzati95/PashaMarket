using Domain.Contracts.Repositories.DiscountAggregate;
using Domain.Contracts.Repositories.ProductAggregate;

namespace Domain;
public interface IUnitOfWork : IDisposable
{

    #region ProductAggRepo

    IProductRepository ProductRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    IProductPictureRepository ProductPictureRepository { get; }
    ISlideRepository SlideRepository { get; }

    #endregion

    #region DiscountAggRepo

    IDiscountRepository DiscountRepository { get; }

    #endregion

    Task<int> CommitAsync();
}
