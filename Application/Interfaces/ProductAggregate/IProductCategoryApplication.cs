using Application.Dtos.ProductAggregate.ProductCategory;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface IProductCategoryApplication
    {
        Task<OperationResult> Create(CreateProductCategory command);
        Task<OperationResult> Edit(EditProductCategory command);
        Task<EditProductCategory> GetDetails(long id);
        Task<List<ProductCategoryViewModel>> GetProductCategories();
        Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel);
        Task<List<MostPopularProductCategoriesQueryModel>> GetMostPopularProductCategoriesQuery();
        Task<List<ProductCategoryQueryModel>> GetCategoriesQuery();

    }
}
