using Application.Dtos.ContactUsAggregate;
using Application.Interfaces.ContactUsAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class ContactUsModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        private readonly IContactUsApplication _contactUsApplication;
        public ContactUsModel(IContactUsApplication contactUsApplication)
        {
            _contactUsApplication = contactUsApplication;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostContactUs(CreateContactUs command, CancellationToken cancellationToken = default)
        {
            var result = await _contactUsApplication.Create(command, cancellationToken);
            //if (!result.IsSucceeded)
            Message = result.Message;
            return Page();
        }
    }
}