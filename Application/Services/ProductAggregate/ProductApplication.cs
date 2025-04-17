﻿using Application.Dtos.ProductAggregate.Product;
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
                    command.ShortDescription, command.Description, picturePath,
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

                var picturePath = await _fileUploader.Upload(command.Picture, path);
                product.Edit(command.Name, command.Code, command.Brand, command.UnitPrice, command.Count,
                    command.ShortDescription, command.Description, picturePath,
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
                    ShortDescription = product.ShortDescription,
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
                                         x.Discounts.FirstOrDefault().DiscountRate : null
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
                if (product == null)

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
                    CreationDate = x.CreateDateTime.ToFarsi()
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

        public async Task<List<SearchProductsQueryModel>> SearchProduct(string value)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllWithIncludesAndThenInCludes(
                                        predicate: x => x.Name.Contains(value) || x.Brand.Contains(value),
                                        orderBy: x => x.OrderByDescending(p => p.CreateDateTime),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: null);

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
    }
}