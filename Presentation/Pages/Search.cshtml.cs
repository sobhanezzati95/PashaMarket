using Application.Dtos.ProductAggregate.Product;
using Application.Dtos.ProductAggregate.ProductCategory;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;
public class SearchModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
    : PageModel
{
    public string? Value = null;
    public ProductSearchQuery? SearchParameterModel = null;
    public List<SearchProductsQueryModel> SearchProductsQueryModel;
    public List<ProductCategoryViewModel> ProductCategories;
    public async Task OnGet([FromQuery] ProductSearchQuery query, CancellationToken cancellationToken = default)
    {
        Value = query.SearchKey;
        SearchParameterModel = new ProductSearchQuery(query.SearchKey, query.MinPrice, query.MaxPrice, query.Exist, query.Sort, query.Categories);
        ProductCategories = await productCategoryApplication.GetProductCategories(cancellationToken);
        SearchProductsQueryModel = await productApplication.SearchProduct(query, cancellationToken);
    }
}