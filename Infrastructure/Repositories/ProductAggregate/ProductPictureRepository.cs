using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductAggregate
{
    public class ProductPictureRepository : BaseRepository<ProductPicture>, IProductPictureRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductPictureRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProductPicture> GetWithProductAndCategory(long id)
        {
            return await _context.ProductPictures
                .Include(x => x.Product)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
