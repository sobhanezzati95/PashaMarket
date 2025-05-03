using Application.Dtos.ProductAggregate.Product;
using Application.Dtos.ProductAggregate.ProductFeature;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductSearchModel SearchModel;
        public List<ProductViewModel> Products;
        public SelectList ProductCategories;

        private readonly IProductApplication _productApplication;
        private readonly IProductCategoryApplication _productCategoryApplication;
        private readonly IProductFeatureApplication _productFeatureApplication;

        public IndexModel(IProductApplication ProductApplication, IProductCategoryApplication productCategoryApplication, IProductFeatureApplication productFeatureApplication)
        {
            _productApplication = ProductApplication;
            _productCategoryApplication = productCategoryApplication;
            _productFeatureApplication = productFeatureApplication;
        }

        public async Task OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(await _productCategoryApplication.GetProductCategories(), "Id", "Name");
            Products = await _productApplication.Search(searchModel);
        }

        public async Task<IActionResult> OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = await _productCategoryApplication.GetProductCategories()
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateProduct command)
        {
            var result = await _productApplication.Create(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id)
        {
            var product = await _productApplication.GetDetails(id);
            product.Categories = await _productCategoryApplication.GetProductCategories();
            return Partial("Edit", product);
        }

        public async Task<JsonResult> OnPostEdit(EditProduct command)
        {
            var result = await _productApplication.Edit(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetNotInStock(long id)
        {
            var result = await _productApplication.NotInStock(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetIsInStock(long id)
        {
            var result = await _productApplication.InStock(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetFeatures(long id)
        {
            var productFeatures = await _productFeatureApplication.GetDetails(id);
            return Partial("Features", productFeatures);
        }

        public async Task<JsonResult> OnPostFeatures(CreateProductFeature command)
        {
            var result = await _productFeatureApplication.Create(command);
            return new JsonResult(result);
        }
    }
}