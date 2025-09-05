using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class ConfirmEmailModel(IUserApplication userApplication) : PageModel
{
    [TempData]
    public string? Message { get; set; }
    public async Task<IActionResult> OnGet(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId)
         || string.IsNullOrWhiteSpace(token))
            return BadRequest();
        var result = await userApplication.ConfirmEmail(userId, token);
        if (result.IsSucceeded)
            return RedirectToPage("/Login");

        Message = result.Message;
        return Page();
    }
}