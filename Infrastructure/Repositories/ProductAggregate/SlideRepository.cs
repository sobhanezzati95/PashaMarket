using Domain.Contracts.Repositories.ProductAggregate;
using Domain.Entities.ProductAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.ProductAggregate
{
    public class SlideRepository : BaseRepository<Slide>, ISlideRepository
    {
        private readonly ApplicationDbContext _context;

        public SlideRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
