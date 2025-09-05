using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class ForgetPasswordModel(IUserApplication userApplication) : PageModel
{
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost(ForgetPassword command, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();
        var result = await userApplication.ForgetPassword(command, cancellationToken);
        return result.IsSucceeded
             ? RedirectToPage("ForgetPasswordEmailSent")
             : Page();
    }
}