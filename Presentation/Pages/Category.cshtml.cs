using Application.Dtos.ProductAggregate.Product;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class CategoryModel(IProductApplication productApplication) : PageModel
{
    public ProductCategoryQueryModel ProductCategories;
    public async Task OnGet(string id, CancellationToken cancellationToken = default)
    {
        ProductCategories = await productApplication.GetProductCategoriesBy(id, cancellationToken);
    }
}