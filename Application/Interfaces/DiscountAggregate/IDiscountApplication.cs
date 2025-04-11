using Application.Dtos.DiscountAggregate;
using Framework.Application;

namespace Application.Interfaces.DiscountAggregate
{
    public interface IDiscountApplication
    {
        Task<OperationResult> Define(DefineDiscount command);
        Task<OperationResult> Edit(EditDiscount command);
        Task<EditDiscount> GetDetails(long id);
        Task<List<DiscountViewModel>> Search(DiscountSearchModel searchModel);
    }
}
