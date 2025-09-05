using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class ResetPasswordModel(IUserApplication userApplication) : PageModel
{
    public ResetPassword ResetPassword { get; set; }
    public IActionResult OnGet(string userId, string token)
    {
        ResetPassword = new ResetPassword { Token = token, UserId = userId };
        return Page();
    }
    public async Task<IActionResult> OnPost(ResetPassword command)
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await userApplication.ResetPassword(command);
        return result.IsSucceeded
            ? RedirectToPage("ResetPasswordSuccess")
            : Page();
    }
}