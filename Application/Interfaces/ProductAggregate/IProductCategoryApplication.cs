using Application.Dtos.ProductAggregate.ProductCategory;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate;
public interface IProductCategoryApplication
{
    Task<OperationResult> Create(CreateProductCategory command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditProductCategory command, CancellationToken cancellationToken = default);
    Task<EditProductCategory> GetDetails(long id, CancellationToken cancellationToken = default);
    Task<List<ProductCategoryViewModel>> GetProductCategories(CancellationToken cancellationToken = default);
    Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel, CancellationToken cancellationToken = default);
    Task<List<MostPopularProductCategoriesQueryModel>> GetMostPopularProductCategoriesQuery(CancellationToken cancellationToken = default);
    Task<List<ProductCategoryQueryModel>> GetCategoriesQuery(CancellationToken cancellationToken = default);
    Task<OperationResult> NotInStock(long id, CancellationToken cancellationToken = default);
    Task<OperationResult> InStock(long id, CancellationToken cancellationToken = default);
}