using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class LoginModel(IUserApplication userApplication) : PageModel
{
    [TempData]
    public string Message { get; set; }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostLogin(Login command, CancellationToken cancellationToken = default)
    {
        var result = await userApplication.Login(command, cancellationToken);
        if (result.IsSucceeded)
            return RedirectToPage("/Index");

        Message = result.Message;
        return Page();
    }
}