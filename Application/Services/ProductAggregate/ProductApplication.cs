using Application.Dtos.OrderAggregate;
using Application.Dtos.ProductAggregate.Product;
using Application.Dtos.ProductAggregate.ProductFeature;
using Application.Interfaces.ProductAggregate;
using Domain.Contracts;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate
{
    public class ProductApplication(IUnitOfWork unitOfWork,
                                    IFileUploader fileUploader,
                                    ILogger<ProductApplication> logger)
        : IProductApplication
    {
        public async Task<OperationResult> Create(CreateProduct command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await unitOfWork.ProductRepository.Exists(x => x.Name == command.Name, cancellationToken))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var categorySlug = await unitOfWork.ProductCategoryRepository.GetSlugById(command.CategoryId);
                var path = $"{categorySlug}//{slug}";
                var picturePath = await fileUploader.Upload(command.Picture, path, cancellationToken);
                var product = Product.Create(command.Name, command.Code, command.Brand, command.UnitPrice, command.Count,
                    command.Description, picturePath,
                    command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                    command.Keywords, command.MetaDescription);
                await unitOfWork.ProductRepository.Add(product, cancellationToken);
                await unitOfWork.CommitAsync(cancellationToken);
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                logger.LogError(e,
                        "#ProductApplication.Create.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }
        public async Task<OperationResult> Edit(EditProduct command, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetProductWithCategory(command.Id, cancellationToken);
                if (product == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (await unitOfWork.ProductRepository.Exists(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var path = $"{product.Category.Slug}/{slug}";
                var picturePath = command.Picture != null
                    ? await fileUploader.Upload(command.Picture, path, cancellationToken)
                    : null;

                product.Edit(command.Name, command.Code, command.Brand, command.UnitPrice, command.Count,
                    command.Description, picturePath,
                    command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                    command.Keywords, command.MetaDescription);
                await unitOfWork.ProductRepository.Update(product);
                await unitOfWork.CommitAsync(cancellationToken);
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.Edit.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }
        public async Task<EditProduct> GetDetails(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetById(id, cancellationToken);
                return new EditProduct
                {
                    Id = product.Id,
                    Name = product.Name,
                    Code = product.Code,
                    Slug = product.Slug,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    Keywords = product.Keywords,
                    MetaDescription = product.MetaDescription,
                    PictureAlt = product.PictureAlt,
                    PictureTitle = product.PictureTitle,
                    Brand = product.Brand,
                    UnitPrice = product.UnitPrice
                };
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<List<LatestProductsQueryModel>> GetLatestProductsQuery(CancellationToken cancellationToken = default)
        {
            try
            {
                var query = await unitOfWork.ProductRepository.GetAllWithIncludesAndThenIncludes(
                                        predicate: null,
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(z => z.Discounts));

                if (await query.AnyAsync() == false)
                    return [];

                var result = await query.Select(x => new LatestProductsQueryModel
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    PictureAlt = x.PictureAlt,
                    UnitPriceAfterDiscount = x.Discounts.FirstOrDefault() != null &&
                                         x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                         x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                         (x.UnitPrice - (x.UnitPrice * x.Discounts.FirstOrDefault().DiscountRate) / 100) : null,
                    Picture = x.Picture,
                    UnitPrice = x.UnitPrice,
                    DiscountPercentage = x.Discounts.FirstOrDefault() != null &&
                                         x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                         x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                         x.Discounts.FirstOrDefault().DiscountRate : null,
                    IsInStock = x.IsInStock
                }).Take(6)
                  .ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.GetLatestProductsQuery.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<List<ProductViewModel>> GetProducts(CancellationToken cancellationToken = default)
        {
            try
            {
                var products = await unitOfWork.ProductRepository.GetAllAsQueryable(cancellationToken);
                return await products.Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<OperationResult> InStock(long productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetById(productId, cancellationToken);
                product.InStock();
                await unitOfWork.ProductRepository.Update(product);
                await unitOfWork.CommitAsync(cancellationToken);
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.InStock.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<OperationResult> NotInStock(long productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetById(productId, cancellationToken);
                product.NotInStock();
                await unitOfWork.ProductRepository.Update(product);
                await unitOfWork.CommitAsync(cancellationToken);
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.NotInStock.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<List<ProductViewModel>> Search(ProductSearchModel searchModel, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = await unitOfWork.ProductRepository.GetAllWithIncludesAndThenIncludes(
                                predicate: null,
                                orderBy: x => x.OrderByDescending(p => p.Id),
                                isTracking: false,
                                ignoreQueryFilters: false,
                                includeProperties: null,
                                thenInclude: query => query.Include(x => x.Category));

                if (await query.AnyAsync() == false)
                    return [];
                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                    query = query.Where(x => x.Name.Contains(searchModel.Name));
                if (searchModel.CategoryId != 0)
                    query = query.Where(x => x.CategoryId == searchModel.CategoryId);
                return await query.Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category.Name,
                    CategoryId = x.CategoryId,
                    Code = x.Code,
                    Picture = x.Picture,
                    UnitPrice = x.UnitPrice,
                    IsInStock = x.IsInStock,
                    Brand = x.Brand,
                    CreationDate = x.CreateDateTime.ToFarsi(),
                    Count = x.Count
                }).ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductApplication.Search.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<ProductCategoryQueryModel> GetProductCategoriesBy(string slug, CancellationToken cancellationToken = default)
        {
            try
            {
                var category = await unitOfWork.ProductCategoryRepository.GetBySlug(slug, cancellationToken);
                var products = await unitOfWork.ProductRepository.GetAllWithIncludesAndThenIncludes(
                                        predicate: x => x.CategoryId == category.Id,
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(z => z.Discounts));

                if (await products.AnyAsync() == false)
                    return new();

                var result = new ProductCategoryQueryModel
                {
                    Name = category.Name,
                    Picture = category.Picture,
                    PictureAlt = category.PictureAlt,
                    PictureTitle = category.PictureTitle,
                    Slug = category.Slug,
                    ProductQueryModels = await products.Select(x => new ProductQueryModel
                    {
                        Name = x.Name,
                        Slug = x.Slug,
                        PictureAlt = x.PictureAlt,
                        UnitPriceAfterDiscount = x.Discounts.FirstOrDefault() != null &&
                                         x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                         x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                         (x.UnitPrice - (x.UnitPrice * x.Discounts.FirstOrDefault().DiscountRate) / 100) : null,
                        Picture = x.Picture,
                        UnitPrice = x.UnitPrice,
                        Brand = x.Brand,
                        DiscountPercentage = x.Discounts.FirstOrDefault() != null &&
                                         x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                         x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                         x.Discounts.FirstOrDefault().DiscountRate : null
                    }).ToListAsync(cancellationToken)
                };
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductCategoryApplication.GetProductCategoriesBy.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<List<SearchProductsQueryModel>> SearchProduct(ProductSearchQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var products = await unitOfWork.ProductRepository.GetAllWithIncludesAndThenIncludes(
                                        predicate: null,
                                        orderBy: null,
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: null);

                if (!string.IsNullOrWhiteSpace(query.SearchKey))
                    products = products.Where(x => x.Name.Contains(query.SearchKey));

                if (query.MinPrice != null)
                    products = products.Where(x => x.UnitPrice > query.MinPrice);

                if (query.MaxPrice != null)
                    products = products.Where(x => x.UnitPrice < query.MaxPrice);

                if (query.Categories != null)
                    products = products.Where(x => query.Categories.Contains(x.CategoryId));

                if (query.Sort != null)
                {
                    if (query.Sort == SortFilterParam.MostPopular || query.Sort == SortFilterParam.BestSelling)
                        products = products.OrderBy(x => x.CreateDateTime);
                    if (query.Sort == SortFilterParam.Cheapest)
                        products = products.OrderBy(x => x.UnitPrice);
                    if (query.Sort == SortFilterParam.MostExpensive)
                        products = products.OrderByDescending(x => x.UnitPrice);
                }

                return await products.Select(x => new SearchProductsQueryModel
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    PictureAlt = x.PictureAlt,
                    UnitPriceAfterDiscount = x.Discounts.FirstOrDefault() != null &&
                                     x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                     x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                     (x.UnitPrice - (x.UnitPrice * x.Discounts.FirstOrDefault().DiscountRate) / 100) : null,
                    Picture = x.Picture,
                    UnitPrice = x.UnitPrice,
                    DiscountPercentage = x.Discounts.FirstOrDefault() != null &&
                                     x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                     x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                     x.Discounts.FirstOrDefault().DiscountRate : null,
                    PictureTitle = x.PictureTitle
                }).ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductCategoryApplication.SearchProduct.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<ProductDetailQueryModel> GetProductDetails(string slug, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = await unitOfWork.ProductRepository.GetAllWithIncludesAndThenIncludes(
                                        predicate: x => x.Slug == slug,
                                        orderBy: null,
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(p => p.Category)
                                                           .Include(p => p.ProductPictures)
                                                           .Include(p => p.Discounts)
                                                           .Include(p => p.ProductFeatures));

                if (await query.AnyAsync(cancellationToken) == false)
                    return new();

                var product = query.FirstOrDefault();
                bool hasDiscount = product.Discounts.FirstOrDefault() != null &&
                                   product.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                   product.Discounts.FirstOrDefault().EndDate >= DateTime.Now;

                return new ProductDetailQueryModel
                {
                    Id = product.Id,
                    Brand = product.Brand,
                    CategorySlug = product.Category.Slug,
                    CategoryId = product.CategoryId,
                    Category = product.Category.Name,
                    Slug = product.Slug,
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                    IsInStock = product.IsInStock,
                    Description = product.Description,
                    Keywords = product.Keywords,
                    Count = product.Count,
                    MetaDescription = product.MetaDescription,
                    ProductPictures = product.ProductPictures
                                        .Select(x => new Dtos.ProductAggregate.ProductPicture.ProductPictureViewModel
                                        {
                                            Picture = x.Picture,
                                            PictureTitle = x.PictureTitle,
                                            PictureAlt = x.PictureAlt,
                                        }).ToList(),
                    ProductFeatures = product.ProductFeatures
                                        .Select(x => new ProductFeatureViewModel
                                        {
                                            DisplayName = x.DisplayName,
                                            Value = x.Value,
                                        }).ToList(),
                    Picture = product.Picture,
                    DiscountPercentage = hasDiscount ?
                                         product.Discounts.FirstOrDefault().DiscountRate : null,
                    UnitPriceAfterDiscount = hasDiscount ?
                                         (product.UnitPrice - (product.UnitPrice * product.Discounts.FirstOrDefault().DiscountRate) / 100) : product.UnitPrice,
                    DiscountAmount = hasDiscount ?
                                     (product.UnitPrice * product.Discounts.FirstOrDefault().DiscountRate) / 100 : 0,
                };
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductCategoryApplication.GetProductDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<List<RelatedProductsQueryModel>> GetRelatedProductsQuery(string categorySlug, long currentProductId, CancellationToken cancellationToken = default)
        {
            try
            {
                var category = await unitOfWork.ProductCategoryRepository.GetBySlug(categorySlug, cancellationToken);
                var products = await unitOfWork.ProductRepository.GetAllWithIncludesAndThenIncludes(
                                        predicate: x => x.CategoryId == category.Id && x.Id != currentProductId,
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(z => z.Discounts));

                if (await products.AnyAsync(cancellationToken) == false)
                    return [];

                var result = await products.Select(x => new RelatedProductsQueryModel
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    PictureAlt = x.PictureAlt,
                    UnitPriceAfterDiscount = x.Discounts.FirstOrDefault() != null &&
                                         x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                         x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                         (x.UnitPrice - (x.UnitPrice * x.Discounts.FirstOrDefault().DiscountRate) / 100) : x.UnitPrice,
                    Picture = x.Picture,
                    UnitPrice = x.UnitPrice,
                    DiscountPercentage = x.Discounts.FirstOrDefault() != null &&
                                         x.Discounts.FirstOrDefault().StartDate <= DateTime.Now &&
                                         x.Discounts.FirstOrDefault().EndDate >= DateTime.Now ?
                                         x.Discounts.FirstOrDefault().DiscountRate : null,
                    PictureTitle = x.PictureTitle
                }).ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e,
               "#ProductCategoryApplication.GetRelatedProductsQuery.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<List<CartItem>> CheckInventoryStatus(List<CartItem> cartItems, CancellationToken cancellationToken = default)
        {
            if (cartItems.Count == 0)
                return [];

            var products = (await unitOfWork.ProductRepository.GetAllAsQueryable()).ToList();
            foreach (var item in cartItems)
            {
                if (products.Any(x => x.Id == item.Id && x.IsInStock && x.Count >= item.Count))
                    item.IsInStock = true;
            }
            return cartItems;
        }
        public async Task UpdateProductViewCount(long productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetById(productId);
                if (product != null)
                {
                    product.UpdateViewCount();
                    await unitOfWork.ProductRepository.Update(product);
                    await unitOfWork.CommitAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e,
                "#ProductCategoryApplication.UpdateProductViewCount.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}