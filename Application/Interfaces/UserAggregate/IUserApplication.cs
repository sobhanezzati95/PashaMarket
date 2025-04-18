using Application.Dtos.UserAggregate.User;
using Framework.Application;

namespace Application.Interfaces.UserAggregate
{
    public interface IUserApplication
    {
        Task<UserViewModel> GetUserBy(long id);
        Task<OperationResult> Register(RegisterUser command);
        Task<OperationResult> Edit(EditUser command);
        Task<OperationResult> ChangePassword(ChangePassword command);
        Task<OperationResult> Login(Login command);
        Task<EditUser> GetDetails(long id);
        Task<List<UserViewModel>> Search(UserSearchModel searchModel);
        Task Logout();
        Task<List<UserViewModel>> GetUsers();
    }
}
