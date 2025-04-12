using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductAggregate
{
    public class ProductCategoryRepository : BaseRepository<ProductCategory>, IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ProductCategory> GetBySlug(string slug)
        {
            return await _context.ProductCategories.FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<string> GetSlugById(long id)
        {
            return (await _context.ProductCategories.Select(x => new { x.Id, x.Slug }).FirstOrDefaultAsync(x => x.Id == id)).Slug;
        }

    }
}
