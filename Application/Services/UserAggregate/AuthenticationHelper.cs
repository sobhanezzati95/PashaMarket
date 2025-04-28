using Framework.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services.UserAggregate
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        //public AuthenticationViewModel CurrentAccountInfo()
        //{
        //    var result = new AuthViewModel();
        //    if (!IsAuthenticated())
        //        return result;

        //    var claims = _contextAccessor.HttpContext.User.Claims.ToList();
        //    result.Id = long.Parse(claims.FirstOrDefault(x => x.Type == "AccountId").Value);
        //    result.Username = claims.FirstOrDefault(x => x.Type == "Username").Value;
        //    result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
        //    result.Fullname = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        //    result.Role = Roles.GetRoleBy(result.RoleId);
        //    return result;
        //}


        public long CurrentUserId()
        {
            return IsAuthenticated()
                ? long.Parse(_contextAccessor.HttpContext.User.Claims.First(x => x.Type == "UserId")?.Value)
                : 0;
        }

        //public string CurrentAccountMobile()
        //{
        //    return IsAuthenticated()
        //        ? _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Mobile")?.Value
        //        : "";
        //}

        public string CurrentAccountRole()
        {
            if (IsAuthenticated())
                return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return null;
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task Signin(AuthenticationViewModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                //new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim("Username", user.Username),
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim("Email", user.Email));


            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);
        }

        public async Task SignOut()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
