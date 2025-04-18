using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IUserApplication _userApplication;

        public LogoutModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public async Task<IActionResult> OnGet()
        {
            await _userApplication.Logout();
            return RedirectToPage("/Index");

        }
    }
}
