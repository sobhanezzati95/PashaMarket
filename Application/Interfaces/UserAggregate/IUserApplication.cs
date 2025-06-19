using Application.Dtos.UserAggregate.User;
using Framework.Application;

namespace Application.Interfaces.UserAggregate;
public interface IUserApplication
{
    Task<UserViewModel> GetUserBy(long id, CancellationToken cancellationToken = default);
    Task<OperationResult> Register(RegisterUser command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditUser command, CancellationToken cancellationToken = default);
    Task<OperationResult> ChangePassword(ChangePassword command, CancellationToken cancellationToken = default);
    Task<OperationResult> Login(Login command,CancellationToken cancellationToken = default);
    Task<EditUser> GetDetails(long id, CancellationToken cancellationToken = default);
    Task<List<UserViewModel>> Search(UserSearchModel searchModel, CancellationToken cancellationToken = default);
    Task Logout();
    Task<List<UserViewModel>> GetUsers(CancellationToken cancellationToken = default);
}