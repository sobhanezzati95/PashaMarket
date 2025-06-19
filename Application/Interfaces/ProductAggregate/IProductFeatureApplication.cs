using Application.Dtos.ProductAggregate.ProductFeature;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate;
public interface IProductFeatureApplication
{
    Task<OperationResult> Create(CreateProductFeature command, CancellationToken cancellationToken = default);
    Task<List<ProductFeatureViewModel>> GetDetails(long productId, CancellationToken cancellationToken = default);
}