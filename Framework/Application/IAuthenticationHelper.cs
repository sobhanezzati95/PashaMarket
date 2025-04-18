namespace Framework.Application
{
    public interface IAuthenticationHelper
    {
        Task SignOut();
        bool IsAuthenticated();
        Task Signin(AuthenticationViewModel account);
        string CurrentAccountRole();
        //AuthenticationViewModel CurrentAccountInfo();
        //long CurrentAccountId();
        //string CurrentAccountMobile();
    }
    public class AuthenticationViewModel
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string Role { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public AuthenticationViewModel()
        {
        }

        public AuthenticationViewModel(long id, long roleId, string fullname, string username, string email)
        {
            Id = id;
            RoleId = roleId;
            Fullname = fullname;
            Username = username;
            Email = email;
        }
    }
}
