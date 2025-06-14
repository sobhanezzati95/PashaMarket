using Application.Dtos.OrderAggregate;
using Application.Dtos.ProductAggregate.Product;
using Application.Dtos.ProductAggregate.ProductFeature;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate
{
    public class ProductApplication : IProductApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductApplication> _logger;

        public ProductApplication(IUnitOfWork unitOfWork, IFileUploader fileUploader, ILogger<ProductApplication> logger)
        {
            _fileUploader = fileUploader;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateProduct command)
        {
            try
            {
                if (await _unitOfWork.ProductRepository.Exists(x => x.Name == command.Name))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var categorySlug = await _unitOfWork.ProductCategoryRepository.GetSlugById(command.CategoryId);
                var path = $"{categorySlug}//{slug}";
                var picturePath = await _fileUploader.Upload(command.Picture, path);
                var product = Product.Create(command.Name, command.Code, command.Brand, command.UnitPrice, command.Count,
                    command.Description, picturePath,
                    command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                    command.Keywords, command.MetaDescription);
                await _unitOfWork.ProductRepository.Add(product);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                        "#ProductApplication.Create.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }

        public async Task<OperationResult> Edit(EditProduct command)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductWithCategory(command.Id);
                if (product == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.ProductRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var path = $"{product.Category.Slug}/{slug}";

                var picturePath = command.Picture != null
                    ? await _fileUploader.Upload(command.Picture, path)
                    : null;

                product.Edit(command.Name, command.Code, command.Brand, command.UnitPrice, command.Count,
                    command.Description, picturePath,
                    command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                    command.Keywords, command.MetaDescription);
                await _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.Edit.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }

        public async Task<EditProduct> GetDetails(long id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(id);
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
                _logger.LogError(e,
               "#ProductApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<LatestProductsQueryModel>> GetLatestProductsQuery()
        {
            try
            {
                var query = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
                                        predicate: null,
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(z => z.Discounts));

                if (query.Any() == false)
                    return new();

                var result = query.Select(x => new LatestProductsQueryModel
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
                })
                    .Take(6)
                    .ToList();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.GetLatestProductsQuery.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductViewModel>> GetProducts()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsQueryable();

                return products.Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> InStock(long productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(productId);
                product.InStock();
                await _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.InStock.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> NotInStock(long productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(productId);
                product.NotInStock();
                await _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();

            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.NotInStock.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductViewModel>> Search(ProductSearchModel searchModel)
        {
            try
            {
                var query = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
                                predicate: null,
                                orderBy: x => x.OrderByDescending(p => p.Id),
                                isTracking: false,
                                ignoreQueryFilters: false,
                                includeProperties: null,
                                thenInclude: query => query.Include(x => x.Category));

                if (query.Any() == false)
                    return new();

                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                    query = query.Where(x => x.Name.Contains(searchModel.Name));

                if (!string.IsNullOrWhiteSpace(searchModel.Code))
                    query = query.Where(x => x.Code.Contains(searchModel.Code));

                if (searchModel.CategoryId != 0)
                    query = query.Where(x => x.CategoryId == searchModel.CategoryId);

                return query.Select(x => new ProductViewModel
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
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.Search.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<ProductCategoryQueryModel> GetProductCategoriesBy(string slug)
        {
            try
            {
                var category = await _unitOfWork.ProductCategoryRepository.GetBySlug(slug);

                var products = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
                                        predicate: x => x.CategoryId == category.Id,
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(z => z.Discounts));

                if (products.Any() == false)
                    return new();

                var result = new ProductCategoryQueryModel
                {
                    Name = category.Name,
                    Picture = category.Picture,
                    PictureAlt = category.PictureAlt,
                    PictureTitle = category.PictureTitle,
                    Slug = category.Slug,
                    ProductQueryModels = products.Select(x => new ProductQueryModel
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
                    }).ToList()
                };

                return result;

            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetProductCategoriesBy.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<SearchProductsQueryModel>> SearchProduct(ProductSearchQuery query)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
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

                return products.Select(x => new SearchProductsQueryModel
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
                })
            .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.SearchProduct.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<ProductDetailQueryModel> GetProductDetails(string slug)
        {
            try
            {
                var query = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
                                        predicate: x => x.Slug == slug,
                                        orderBy: null,
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(p => p.Category)
                                                           .Include(p => p.ProductPictures)
                                                           .Include(p => p.Discounts)
                                                           .Include(p => p.ProductFeatures)
                                        );

                if (query.Any() == false)
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
                _logger.LogError(e,
               "#ProductCategoryApplication.GetProductDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<RelatedProductsQueryModel>> GetRelatedProductsQuery(string categorySlug, long currentProductId)
        {
            try
            {
                var category = await _unitOfWork.ProductCategoryRepository.GetBySlug(categorySlug);

                var products = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
                                        predicate: x => x.CategoryId == category.Id && x.Id != currentProductId,
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: x => x.Include(z => z.Discounts));

                if (products.Any() == false)
                    return new();

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
                }).ToListAsync();

                return result;

            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetRelatedProductsQuery.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<CartItem>> CheckInventoryStatus(List<CartItem> cartItems)
        {
            if (!cartItems.Any())
                return new();

            var products = (await _unitOfWork.ProductRepository.GetAllAsQueryable()).ToList();

            foreach (var item in cartItems)
            {
                if (products.Any(x => x.Id == item.Id && x.IsInStock && x.Count >= item.Count))
                {
                    item.IsInStock = true;
                }

            }
            return cartItems;
        }
    }
}