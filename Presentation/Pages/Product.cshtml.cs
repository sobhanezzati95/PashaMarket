using Application.Dtos.ProductAggregate.Product;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class ProductModel : PageModel
    {
        public ProductDetailQueryModel Product;
        public List<RelatedProductsQueryModel> RelatedProducts = new();

        private readonly IProductApplication _productApplication;

        public ProductModel(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }


        public async Task OnGet(string id, CancellationToken cancellationToken = default)
        {
            Product = await _productApplication.GetProductDetails(id, cancellationToken);
            if (Product != null)
                RelatedProducts = await _productApplication.GetRelatedProductsQuery(Product.CategorySlug, Product.Id, cancellationToken);
        }
    }
}