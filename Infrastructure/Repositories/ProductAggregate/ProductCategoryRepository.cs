using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductAggregate;
public class ProductCategoryRepository(ApplicationDbContext context)
    : BaseRepository<ProductCategory>(context), IProductCategoryRepository
{
    public async Task<ProductCategory> GetBySlug(string slug, CancellationToken cancellationToken = default)
        => await context.ProductCategories.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken)
            ?? throw new Exception();
    public async Task<string> GetSlugById(long id, CancellationToken cancellationToken = default)
    {
        var productCategory = await context.ProductCategories.AsNoTracking()
                                                             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                              ?? throw new Exception();
        return productCategory.Slug;
    }
}