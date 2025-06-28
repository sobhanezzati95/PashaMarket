using Domain.Contracts.Repositories.UserAggregate;
using Domain.Entities.UserAggregate;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories.UserAggregate;
public class RoleRepository(ApplicationDbContext context)
    : BaseRepository<Role>(context), IRoleRepository
{ }