using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents;
public class FooterViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(View());
    }
}