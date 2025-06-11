using Domain.Contracts.Repositories.ContactUsAggregate;
using Domain.Entities.ContactUsAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.ContactUsAggregate
{
    public class ContactUsRepository : BaseRepository<ContactUs>, IContactUsRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactUsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
