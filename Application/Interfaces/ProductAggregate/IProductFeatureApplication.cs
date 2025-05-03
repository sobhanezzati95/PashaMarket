using Application.Dtos.ProductAggregate.ProductFeature;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface IProductFeatureApplication
    {
        Task<OperationResult> Create(CreateProductFeature command);
        Task<List<ProductFeatureViewModel>> GetDetails(long productId);

    }
}
