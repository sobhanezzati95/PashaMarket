using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class LogoutModel(IUserApplication userApplication) : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        await userApplication.Logout();
        return RedirectToPage("/Index");
    }
}