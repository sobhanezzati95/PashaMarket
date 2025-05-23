using Application.Dtos.ProductAggregate.ProductCategory;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Admin.Pages.ProductCategories
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductCategorySearchModel SearchModel;
        public List<ProductCategoryViewModel> ProductCategories;

        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        public async Task OnGet(ProductCategorySearchModel searchModel)
        {
            ProductCategories = await _productCategoryApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        public async Task<JsonResult> OnPostCreate(CreateProductCategory command)
        {
            var result = await _productCategoryApplication.Create(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id)
        {
            var productCategory = await _productCategoryApplication.GetDetails(id);
            return Partial("Edit", productCategory);
        }

        public async Task<JsonResult> OnPostEdit(EditProductCategory command)
        {
            var result = await _productCategoryApplication.Edit(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetMakeItUnpopular(long id)
        {
            var result = await _productCategoryApplication.NotInStock(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetMakeItPopular(long id)
        {
            var result = await _productCategoryApplication.InStock(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
