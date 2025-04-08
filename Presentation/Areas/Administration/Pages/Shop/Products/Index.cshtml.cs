using Application.Dtos.ProductAggregate.Product;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Administration.Pages.Shop.Products
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

        public IndexModel(IProductApplication ProductApplication, IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = ProductApplication;
            _productCategoryApplication = productCategoryApplication;
        }

        public async Task OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList((await _productCategoryApplication.GetProductCategories()).Data, "Id", "Name");
            Products = (await _productApplication.Search(searchModel)).Data;
        }

        public async Task<IActionResult> OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = (await _productCategoryApplication.GetProductCategories()).Data
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
            var product = (await _productApplication.GetDetails(id)).Data;
            product.Categories = (await _productCategoryApplication.GetProductCategories()).Data;
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
            if (result.IsSuccedded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetIsInStock(long id)
        {
            var result = await _productApplication.InStock(id);
            if (result.IsSuccedded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
