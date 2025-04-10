using Application.Dtos.ProductAggregate.Slide;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Admin.Pages.Slides
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<SlideViewModel> Slides;

        private readonly ISlideApplication _slideApplication;

        public IndexModel(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }

        public async Task OnGet()
        {
            Slides = await _slideApplication.GetList();
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateSlide();
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateSlide command)
        {
            var result = await _slideApplication.Create(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id)
        {
            var slide = await _slideApplication.GetDetails(id);
            return Partial("Edit", slide);
        }

        public async Task<JsonResult> OnPostEdit(EditSlide command)
        {
            var result = await _slideApplication.Edit(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetRemove(long id)
        {
            var result = await _slideApplication.Remove(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetRestore(long id)
        {
            var result = await _slideApplication.Restore(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
