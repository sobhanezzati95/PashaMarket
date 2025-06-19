using Application.Dtos.ProductAggregate.ProductPicture;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate;
public interface IProductPictureApplication
{
    Task<OperationResult> Create(CreateProductPicture command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditProductPicture command, CancellationToken cancellationToken = default);
    Task<OperationResult> Remove(long id, CancellationToken cancellationToken = default);
    Task<OperationResult> Restore(long id, CancellationToken cancellationToken = default);
    Task<EditProductPicture> GetDetails(long id, CancellationToken cancellationToken = default);
    Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel, CancellationToken cancellationToken = default);
}