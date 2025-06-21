using Application.Dtos.OrderAggregate;
using Application.Dtos.ProductAggregate.Product;
using Framework.Application;

namespace Application.Interfaces.ProductAggregate;
public interface IProductApplication
{
    Task<OperationResult> Create(CreateProduct command, CancellationToken cancellationToken = default);
    Task<OperationResult> Edit(EditProduct command, CancellationToken cancellationToken = default);
    Task<EditProduct> GetDetails(long id, CancellationToken cancellationToken = default);
    Task<List<ProductViewModel>> GetProducts(CancellationToken cancellationToken = default);
    Task<List<ProductViewModel>> Search(ProductSearchModel searchModel, CancellationToken cancellationToken = default);
    Task<OperationResult> InStock(long productId, CancellationToken cancellationToken = default);
    Task<OperationResult> NotInStock(long productId, CancellationToken cancellationToken = default);
    Task<List<LatestProductsQueryModel>> GetLatestProductsQuery(CancellationToken cancellationToken = default);
    Task<ProductCategoryQueryModel> GetProductCategoriesBy(string slug, CancellationToken cancellationToken = default);
    Task<List<SearchProductsQueryModel>> SearchProduct(ProductSearchQuery query, CancellationToken cancellationToken = default);
    Task<ProductDetailQueryModel> GetProductDetails(string slug, CancellationToken cancellationToken = default);
    Task<List<RelatedProductsQueryModel>> GetRelatedProductsQuery(string categorySlug, long currentProductId, CancellationToken cancellationToken = default);
    Task<List<CartItem>> CheckInventoryStatus(List<CartItem> cartItems, CancellationToken cancellationToken = default);
    Task UpdateProductViewCount(long productId, CancellationToken cancellationToken = default);
}