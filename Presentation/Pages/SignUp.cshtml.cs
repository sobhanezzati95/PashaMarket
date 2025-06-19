using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class SignUpModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        private readonly IUserApplication _userApplication;

        public SignUpModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostSignUp(RegisterUser command, CancellationToken cancellationToken = default)
        {
            var result = await _userApplication.Register(command, cancellationToken);
            if (result.IsSucceeded)
                return RedirectToPage("/Index", cancellationToken);

            Message = result.Message;
            return Page();
        }
    }
}