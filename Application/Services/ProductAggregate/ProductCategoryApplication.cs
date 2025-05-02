using Application.Dtos.ProductAggregate.ProductCategory;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductCategoryApplication> _logger;

        public ProductCategoryApplication(IUnitOfWork unitOfWork, IFileUploader fileUploader, ILogger<ProductCategoryApplication> logger)
        {
            _fileUploader = fileUploader;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateProductCategory command)
        {
            try
            {
                if (await _unitOfWork.ProductCategoryRepository.Exists(x => x.Name == command.Name))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var picturePath = $"{command.Slug}";
                var pictureName = await _fileUploader.Upload(command.Picture, picturePath);

                var productCategory = ProductCategory.Create(command.Name, command.Description, pictureName, command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, slug);

                await _unitOfWork.ProductCategoryRepository.Add(productCategory);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                        "#ProductCategoryApplication.Create.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }

        public async Task<OperationResult> Edit(EditProductCategory command)
        {
            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetById(command.Id);
                if (productCategory == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.ProductCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();

                var picturePath = $"{command.Slug}";
                var fileName = await _fileUploader.Upload(command.Picture, picturePath);

                productCategory.Edit(command.Name, command.Description, fileName, command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, slug);

                await _unitOfWork.ProductCategoryRepository.Update(productCategory);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.Edit.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }

        public async Task<EditProductCategory> GetDetails(long id)
        {
            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetById(id);

                return new EditProductCategory
                {
                    Id = productCategory.Id,
                    Description = productCategory.Description,
                    Name = productCategory.Name,
                    Keywords = productCategory.Keywords,
                    MetaDescription = productCategory.MetaDescription,
                    Slug = productCategory.Slug
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductCategoryViewModel>> GetProductCategories()
        {
            try
            {
                var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();

                return productCategories.Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetProductCategories.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel)
        {
            try
            {
                var query = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();

                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                    query = query.Where(x => x.Name.Contains(searchModel.Name));

                return query.Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    CreationDate = x.CreateDateTime.ToFarsi(),
                    Ispopular = x.IsPopular
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.Search.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<MostPopularProductCategoriesQueryModel>> GetMostPopularProductCategoriesQuery()
        {
            try
            {
                var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();

                return productCategories
                    .Where(x => x.IsPopular)
                    .Select(x => new MostPopularProductCategoriesQueryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Picture = x.Picture,
                        PictureAlt = x.PictureAlt,
                        PictureTitle = x.PictureTitle,
                        Slug = x.Slug,
                    }).AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetMostPopularProductCategoriesQuery.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductCategoryQueryModel>> GetCategoriesQuery()
        {
            try
            {
                var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();

                return productCategories.Select(x => new ProductCategoryQueryModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetCategoriesQuery.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> NotInStock(long id)
        {
            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetById(id);
                if (productCategory == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                productCategory.MakeItUnpopular();

                await _unitOfWork.ProductCategoryRepository.Update(productCategory);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductCategoryApplication.NotInStock.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
        public async Task<OperationResult> InStock(long id)
        {
            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetById(id);
                if (productCategory == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                var query = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();
                var count = query.Where(x => x.IsPopular).Count();
                if (count >= 5)
                    return OperationResult.Failed(ApplicationMessages.InvalidOperation);

                productCategory.MakeItPopular();

                await _unitOfWork.ProductCategoryRepository.Update(productCategory);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.InStock.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}
