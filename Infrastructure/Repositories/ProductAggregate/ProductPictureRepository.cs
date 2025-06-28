using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductAggregate;
public class ProductPictureRepository(ApplicationDbContext context)
    : BaseRepository<ProductPicture>(context), IProductPictureRepository
{
    public async Task<ProductPicture> GetWithProductAndCategory(long id, CancellationToken cancellationToken = default)
        => await context.ProductPictures.Include(x => x.Product)
                                        .ThenInclude(x => x.Category)
                                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
          ?? throw new Exception();
}