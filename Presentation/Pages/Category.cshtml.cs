using Application.Dtos.ProductAggregate.Product;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class CategoryModel : PageModel
    {
        public ProductCategoryQueryModel ProductCategories;
        private readonly IProductApplication _productApplication;

        public CategoryModel(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }
        public async Task OnGet(string id, CancellationToken cancellationToken = default)
        {
            ProductCategories = await _productApplication.GetProductCategoriesBy(id, cancellationToken);
        }
    }
}