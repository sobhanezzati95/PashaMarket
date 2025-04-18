using Domain.Entities.UserAggregate;
using Framework.Domain;

namespace Domain.Contracts.Repositories.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsername(string username);
    }
}
