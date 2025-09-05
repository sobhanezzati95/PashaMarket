using Application.Dtos.UserAggregate.User;
using Framework.Application;

namespace Application.Interfaces.UserAggregate;
public interface IUserApplication
{
    Task<OperationResult> Register(RegisterUser command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditUser command, CancellationToken cancellationToken = default);
    Task<OperationResult> ChangePassword(ChangePassword command, CancellationToken cancellationToken = default);
    Task<OperationResult> Login(Login command, CancellationToken cancellationToken = default);
    Task Logout();
    Task<EditUser> GetUserInfo(CancellationToken cancellationToken = default);
    Task<OperationResult> ConfirmEmail(string userId, string token, CancellationToken cancellationToken = default);
    Task<OperationResult> ForgetPassword(ForgetPassword command, CancellationToken cancellationToken);
    Task<OperationResult> ResetPassword(ResetPassword command, CancellationToken cancellationToken = default);
}