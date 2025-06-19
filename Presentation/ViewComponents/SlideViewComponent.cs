using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents;
public class SlideViewComponent(ISlideApplication slideApplication)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var slides = await slideApplication.GetSlides(cancellationToken);
        return View(slides);
    }
}