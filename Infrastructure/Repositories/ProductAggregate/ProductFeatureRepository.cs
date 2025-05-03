using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.ProductAggregate
{
    public class ProductFeatureRepository : BaseRepository<ProductFeature>, IProductFeatureRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductFeatureRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
