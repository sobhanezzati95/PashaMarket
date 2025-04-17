using Application.Dtos.ProductAggregate.Product;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class SearchModel : PageModel
    {
        public string Value;
        public List<SearchProductsQueryModel> SearchProductsQueryModel;
        private readonly IProductApplication _productApplication;

        public SearchModel(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        public async Task OnGet(string value)
        {
            Value = value;
            SearchProductsQueryModel = await _productApplication.SearchProduct(value);
        }
    }
}
