using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class LoginModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        private readonly IUserApplication _userApplication;

        public LoginModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLogin(Login command)
        {
            var result = await _userApplication.Login(command);
            if (result.IsSucceeded)
                return RedirectToPage("/Index");

            Message = result.Message;
            return RedirectToPage("/Login");
        }
    }
}
