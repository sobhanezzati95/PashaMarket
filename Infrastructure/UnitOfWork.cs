using Domain;
using Domain.Contracts.Repositories.ProductAggregate;
using Infrastructure.DbContexts;
using Infrastructure.Repositories.ProductAggregate;

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
    }

    #region ProductAggRepo

    public IProductRepository ProductRepository { get; private set; }
    public IProductCategoryRepository ProductCategoryRepository { get; private set; }
    public IProductPictureRepository ProductPictureRepository { get; private set; }
    public ISlideRepository SlideRepository { get; private set; }

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
