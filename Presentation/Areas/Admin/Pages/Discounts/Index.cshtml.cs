using Application.Dtos.DiscountAggregate;
using Application.Interfaces.DiscountAggregate;
using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Pages.Discounts
{
    //[Authorize(Roles = Roles.Administator)]
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public DiscountSearchModel SearchModel;
        public List<DiscountViewModel> CustomerDiscounts;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly IDiscountApplication _discountApplication;

        public IndexModel(IProductApplication ProductApplication, IDiscountApplication discountApplication)
        {
            _productApplication = ProductApplication;
            _discountApplication = discountApplication;
        }

        public async Task OnGet(DiscountSearchModel searchModel)
        {
            Products = new SelectList(await _productApplication.GetProducts(), "Id", "Name");
            CustomerDiscounts = await _discountApplication.Search(searchModel);
        }

        public async Task<IActionResult> OnGetCreate()
        {
            var command = new DefineDiscount
            {
                Products = await _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        public async Task<JsonResult> OnPostCreate(DefineDiscount command)
        {
            var result = await _discountApplication.Define(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id)
        {
            var customerDiscount = await _discountApplication.GetDetails(id);
            customerDiscount.Products = await _productApplication.GetProducts();
            return Partial("Edit", customerDiscount);
        }

        public async Task<JsonResult> OnPostEdit(EditDiscount command)
        {
            var result = await _discountApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
