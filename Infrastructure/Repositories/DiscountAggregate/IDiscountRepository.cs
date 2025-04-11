using Domain.Entities.DiscountAggregate;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;

namespace Domain.Contracts.Repositories.DiscountAggregate
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
