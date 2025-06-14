using Application.Dtos.ProductAggregate.Product;
using Application.Dtos.ProductAggregate.ProductCategory;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class SearchModel : PageModel
    {
        public string? Value = null;
        public ProductSearchQuery? SearchParameterModel = null;
        public List<SearchProductsQueryModel> SearchProductsQueryModel;
        public List<ProductCategoryViewModel> ProductCategories;
        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;
        public SearchModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        public async Task OnGet([FromQuery] ProductSearchQuery query)
        {
            Value = query.SearchKey;
            SearchParameterModel = new(query.SearchKey, query.MinPrice, query.MaxPrice, query.Exist, query.Sort, query.Categories);
            ProductCategories = await _productCategoryApplication.GetProductCategories();
            SearchProductsQueryModel = await _productApplication.SearchProduct(query);
        }
    }
}
