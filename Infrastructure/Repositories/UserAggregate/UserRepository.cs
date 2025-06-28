using Domain.Contracts.Repositories.UserAggregate;
using Domain.Entities.UserAggregate;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UserAggregate;
public class UserRepository(ApplicationDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
}