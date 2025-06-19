using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductAggregate;
public class ProductRepository(ApplicationDbContext context)
    : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product> GetProductWithCategory(long id, CancellationToken cancellationToken = default)
         => await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
}