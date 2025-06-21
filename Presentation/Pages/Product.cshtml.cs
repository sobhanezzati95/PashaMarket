using Application.Dtos.ProductAggregate.Product;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class ProductModel(IProductApplication productApplication) : PageModel
{
    public ProductDetailQueryModel Product;
    public List<RelatedProductsQueryModel> RelatedProducts = [];
    public async Task OnGet(string id, CancellationToken cancellationToken = default)
    {
        Product = await productApplication.GetProductDetails(id, cancellationToken);
        if (Product != null)
        {
            RelatedProducts = await productApplication.GetRelatedProductsQuery(Product.CategorySlug, Product.Id, cancellationToken);
            await productApplication.UpdateProductViewCount(Product.Id, cancellationToken);
        }
    }
}