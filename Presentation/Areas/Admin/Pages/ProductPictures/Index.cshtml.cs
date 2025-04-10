using Application.Dtos.ProductAggregate.ProductPicture;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Pages.ProductPictures
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ProductPictureSearchModel SearchModel;
        public List<ProductPictureViewModel> ProductPictures;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly IProductPictureApplication _productPictureApplication;
        public IndexModel(IProductApplication ProductApplication, IProductPictureApplication productPictureApplication)
        {
            _productApplication = ProductApplication;
            _productPictureApplication = productPictureApplication;
        }

        public async Task OnGet(ProductPictureSearchModel searchModel)
        {
            Products = new SelectList(await _productApplication.GetProducts(), "Id", "Name");
            ProductPictures = await _productPictureApplication.Search(searchModel);
        }

        public async Task<IActionResult> OnGetCreate()
        {
            var command = new CreateProductPicture
            {
                Products = await _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(CreateProductPicture command)
        {
            var result = await _productPictureApplication.Create(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id)
        {
            var productPicture = await _productPictureApplication.GetDetails(id);
            productPicture.Products = await _productApplication.GetProducts();
            return Partial("Edit", productPicture);
        }

        public async Task<JsonResult> OnPostEdit(EditProductPicture command)
        {
            var result = await _productPictureApplication.Edit(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetRemove(long id)
        {
            var result = await _productPictureApplication.Remove(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetRestore(long id)
        {
            var result = await _productPictureApplication.Restore(id);
            if (result.IsSucceeded)
                return RedirectToPage("./Index");

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
