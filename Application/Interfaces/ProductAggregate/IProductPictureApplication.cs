using Application.Dtos.ProductAggregate.ProductPicture;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface IProductPictureApplication
    {
        Task<OperationResult> Create(CreateProductPicture command);
        Task<OperationResult> Edit(EditProductPicture command);
        Task<OperationResult> Remove(long id);
        Task<OperationResult> Restore(long id);
        Task<EditProductPicture> GetDetails(long id);
        Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel);
    }
}
