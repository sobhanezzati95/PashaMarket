using Application.Dtos.DiscountAggregate;
using Framework.Application;

namespace Application.Interfaces.DiscountAggregate;
public interface IDiscountApplication
{
    Task<OperationResult> Define(DefineDiscount command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditDiscount command, CancellationToken cancellationToken = default);
    Task<EditDiscount> GetDetails(long id, CancellationToken cancellationToken = default);
    Task<List<DiscountViewModel>> Search(DiscountSearchModel searchModel, CancellationToken cancellationToken = default);
}