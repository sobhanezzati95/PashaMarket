using Application.Dtos.ContactUsAggregate;
using Application.Interfaces.ContactUsAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class ContactUsModel(IContactUsApplication contactUsApplication) : PageModel
{
    [TempData]
    public string Message { get; set; }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostContactUs(CreateContactUs command, CancellationToken cancellationToken = default)
    {
        var result = await contactUsApplication.Create(command, cancellationToken);
        Message = result.Message;
        return Page();
    }
}