using Domain.Contracts.Repositories.DiscountAggregate;
using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Contracts.Repositories.UserAggregate;

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

    #region UserAggRepo
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }

    #endregion

    Task<int> CommitAsync();
}
