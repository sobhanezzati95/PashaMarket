using Domain.Contracts.Repositories.ContactUsAggregate;
using Domain.Contracts.Repositories.DiscountAggregate;
using Domain.Contracts.Repositories.OrderAggregate;
using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Contracts.Repositories.UserAggregate;

namespace Domain;
public interface IUnitOfWork : IDisposable
{

    #region ProductAggRepo

    IProductRepository ProductRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    IProductPictureRepository ProductPictureRepository { get; }
    IProductFeatureRepository ProductFeatureRepository { get; }
    ISlideRepository SlideRepository { get; }

    #endregion

    #region DiscountAggRepo

    IDiscountRepository DiscountRepository { get; }

    #endregion

    #region UserAggRepo

    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }

    #endregion

    #region OrderAggRepo

    IOrderRepository OrderRepository { get; }
    IOrderItemRepository OrderItemRepository { get; }

    #endregion

    #region ContactUsAggRepo

    IContactUsRepository ContactUsRepository { get; }

    #endregion

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
