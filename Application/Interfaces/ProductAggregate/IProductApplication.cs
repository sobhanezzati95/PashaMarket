using Application.Dtos.ProductAggregate.Product;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate
{
    public interface IProductApplication
    {
        Task<OperationResult> Create(CreateProduct command);
        Task<OperationResult> Edit(EditProduct command);
        Task<EditProduct> GetDetails(long id);
        Task<List<ProductViewModel>> GetProducts();
        Task<List<ProductViewModel>> Search(ProductSearchModel searchModel);
        Task<OperationResult> InStock(long productId);
        Task<OperationResult> NotInStock(long productId);
        Task<List<LatestProductsQueryModel>> GetLatestProductsQuery();
        Task<ProductCategoryQueryModel> GetProductCategoriesBy(string slug);
        Task<List<SearchProductsQueryModel>> SearchProduct(string value);
        Task<ProductDetailQueryModel> GetProductDetails(string slug);
        Task<List<RelatedProductsQueryModel>> GetRelatedProductsQuery(string categorySlug, long currentProductId);
    }
}
