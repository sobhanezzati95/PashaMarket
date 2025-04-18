using Domain.Contracts.Repositories.UserAggregate;
using Domain.Entities.UserAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.UserAggregate
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
