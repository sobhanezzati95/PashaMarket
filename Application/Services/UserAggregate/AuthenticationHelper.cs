using Framework.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services.UserAggregate;
public class AuthenticationHelper(IHttpContextAccessor contextAccessor)
    : IAuthenticationHelper
{
    public long CurrentUserId()
     => IsAuthenticated()
         ? long.Parse(contextAccessor.HttpContext.User.Claims.First(x => x.Type == "UserId")?.Value)
         : 0;
    public string CurrentAccountRole()
        => IsAuthenticated() ? (contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) : null;
    public bool IsAuthenticated()
        => contextAccessor.HttpContext.User.Identity.IsAuthenticated;
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

        await contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(claimsIdentity),
              authProperties);
    }
    public async Task SignOut()
        => await contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}