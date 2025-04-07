using Application.Dtos.ProductAggregate.ProductCategory;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
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

        public async Task<OperationResult<long>> Create(CreateProductCategory command)
        {
            try
            {
                if (await _unitOfWork.ProductCategoryRepository.Exists(x => x.Name == command.Name))
                    return OperationResult<long>.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var productCategory = ProductCategory.Create(command.Name, command.Description, command.Keywords, command.MetaDescription, slug);

                await _unitOfWork.ProductCategoryRepository.Add(productCategory);
                await _unitOfWork.CommitAsync();
                return OperationResult<long>.Succedded(productCategory.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                        "#ProductCategoryApplication.Create.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<long>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<bool>> Edit(EditProductCategory command)
        {
            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetById(command.Id);
                if (productCategory == null)
                    return OperationResult<bool>.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.ProductCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                    return OperationResult<bool>.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                productCategory.Edit(command.Name, command.Description, command.Keywords, command.MetaDescription, slug);

                await _unitOfWork.ProductCategoryRepository.Update(productCategory);
                await _unitOfWork.CommitAsync();
                return OperationResult<bool>.Succedded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.Edit.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<bool>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<EditProductCategory>> GetDetails(long id)
        {
            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetById(id);
                if (productCategory == null)
                    return OperationResult<EditProductCategory>.Failed(ApplicationMessages.RecordNotFound);

                return OperationResult<EditProductCategory>.Succedded(new EditProductCategory
                {
                    Id = productCategory.Id,
                    Description = productCategory.Description,
                    Name = productCategory.Name,
                    Keywords = productCategory.Keywords,
                    MetaDescription = productCategory.MetaDescription,
                    Slug = productCategory.Slug
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<EditProductCategory>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<List<ProductCategoryViewModel>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();
                if (productCategories.Any() == false)
                    return OperationResult<List<ProductCategoryViewModel>>.Failed(ApplicationMessages.RecordNotFound);

                return OperationResult<List<ProductCategoryViewModel>>.Succedded(productCategories.Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.GetProductCategories.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<List<ProductCategoryViewModel>>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<List<ProductCategoryViewModel>>> Search(ProductCategorySearchModel searchModel)
        {
            try
            {
                var query = await _unitOfWork.ProductCategoryRepository.GetAllAsQueryable();

                if (query.Any() == false)
                    return OperationResult<List<ProductCategoryViewModel>>.Failed(ApplicationMessages.RecordNotFound);

                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                    query = query.Where(x => x.Name.Contains(searchModel.Name));

                return OperationResult<List<ProductCategoryViewModel>>.Succedded(query.Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductCategoryApplication.Search.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<List<ProductCategoryViewModel>>.Failed(e.Message);
            }
        }
    }
}
