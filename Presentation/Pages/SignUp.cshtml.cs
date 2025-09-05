using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class SignUpModel(IUserApplication userApplication) : PageModel
{
    [TempData]
    public string? Message { get; set; }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostSignUp(RegisterUser command, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();
        var result = await userApplication.Register(command, cancellationToken);
        if (result.IsSucceeded)
            return RedirectToPage("/DisplayEmail");
        return Page();
    }
}