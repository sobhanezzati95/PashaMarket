using Domain;
using Domain.Contracts.Repositories.ContactUsAggregate;
using Domain.Contracts.Repositories.DiscountAggregate;
using Domain.Contracts.Repositories.OrderAggregate;
using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Contracts.Repositories.UserAggregate;
using Infrastructure.DbContexts;
using Infrastructure.Repositories.ContactUsAggregate;
using Infrastructure.Repositories.OrderAggregate;
using Infrastructure.Repositories.ProductAggregate;
using Infrastructure.Repositories.UserAggregate;

namespace Infrastructure;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        ProductRepository = new ProductRepository(_dbContext);
        ProductPictureRepository = new ProductPictureRepository(_dbContext);
        ProductCategoryRepository = new ProductCategoryRepository(_dbContext);
        SlideRepository = new SlideRepository(_dbContext);
        DiscountRepository = new DiscountRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
        RoleRepository = new RoleRepository(_dbContext);
        OrderRepository = new OrderRepository(_dbContext);
        OrderItemRepository = new OrderItemRepository(_dbContext);
        ProductFeatureRepository = new ProductFeatureRepository(_dbContext);
        ContactUsRepository = new ContactUsRepository(_dbContext);
    }

    #region ProductAggRepo

    public IProductRepository ProductRepository { get; private set; }
    public IProductCategoryRepository ProductCategoryRepository { get; private set; }
    public IProductPictureRepository ProductPictureRepository { get; private set; }
    public ISlideRepository SlideRepository { get; private set; }
    public IProductFeatureRepository ProductFeatureRepository { get; private set; }

    #endregion

    #region DiscountAggRepo

    public IDiscountRepository DiscountRepository { get; private set; }

    #endregion

    #region UserAggRepo

    public IUserRepository UserRepository { get; private set; }
    public IRoleRepository RoleRepository { get; private set; }

    #endregion

    #region OrderAggRepo

    public IOrderRepository OrderRepository { get; private set; }
    public IOrderItemRepository OrderItemRepository { get; private set; }

    #endregion

    #region ContactUsAggRepo

    public IContactUsRepository ContactUsRepository { get; private set; }

    #endregion

    public Task<int> CommitAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
