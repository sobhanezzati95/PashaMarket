using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductAggregate;
public class ProductCategoryRepository(ApplicationDbContext context)
    : BaseRepository<ProductCategory>(context), IProductCategoryRepository
{
    public async Task<ProductCategory> GetBySlug(string slug, CancellationToken cancellationToken = default)
        => await context.ProductCategories.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    public async Task<string> GetSlugById(long id, CancellationToken cancellationToken = default)
        => (await context.ProductCategories.Select(x => new { x.Id, x.Slug }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken)).Slug;
}
