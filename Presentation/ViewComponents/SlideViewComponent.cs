using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class SlideViewComponent : ViewComponent
    {
        private readonly ISlideApplication _slideApplication;

        public SlideViewComponent(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slides = await _slideApplication.GetSlides();
            return View(slides);
        }
    }
}
