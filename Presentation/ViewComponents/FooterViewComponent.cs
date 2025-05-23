using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
