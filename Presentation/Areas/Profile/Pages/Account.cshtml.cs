using Application.Dtos.UserAggregate.User;
using Application.Interfaces.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Profile.Pages;
public class AccountModel(IUserApplication userApplication) : PageModel
{
    public EditUser editUser;
    public async Task OnGet(CancellationToken cancellationToken = default)
        => editUser = await userApplication.GetUserInfo(cancellationToken);
    public async Task<IActionResult> OnPostAccount(EditUser command, CancellationToken cancellationToken = default)
    {
        var result = await userApplication.Edit(command, cancellationToken);
        return RedirectToPage("/Index");
    }
}