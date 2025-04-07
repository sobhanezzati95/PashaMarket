using Application.Dtos.ProductAggregate.Product;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface IProductApplication
    {
        Task<OperationResult<long>> Create(CreateProduct command);
        Task<OperationResult<bool>> Edit(EditProduct command);
        Task<OperationResult<EditProduct>> GetDetails(long id);
        Task<OperationResult<List<ProductViewModel>>> GetProducts();
        Task<OperationResult<List<ProductViewModel>>> Search(ProductSearchModel searchModel);
    }
}
