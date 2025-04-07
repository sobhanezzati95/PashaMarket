using Application.Dtos.ProductAggregate.ProductCategory;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface IProductCategoryApplication
    {
        Task<OperationResult<long>> Create(CreateProductCategory command);
        Task<OperationResult<bool>> Edit(EditProductCategory command);
        Task<OperationResult<EditProductCategory>> GetDetails(long id);
        Task<OperationResult<List<ProductCategoryViewModel>>> GetProductCategories();
        Task<OperationResult<List<ProductCategoryViewModel>>> Search(ProductCategorySearchModel searchModel);
    }
}
