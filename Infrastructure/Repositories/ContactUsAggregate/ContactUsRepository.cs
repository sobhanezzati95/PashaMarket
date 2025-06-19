using Domain.Contracts.Repositories.ContactUsAggregate;
using Domain.Entities.ContactUsAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.ContactUsAggregate;
public class ContactUsRepository(ApplicationDbContext context)
    : BaseRepository<ContactUs>(context), IContactUsRepository
{ }