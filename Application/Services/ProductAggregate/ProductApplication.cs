using Application.Dtos.ProductAggregate.Product;
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

        public async Task<OperationResult<long>> Create(CreateProduct command)
        {
            try
            {
                if (await _unitOfWork.ProductRepository.Exists(x => x.Name == command.Name))
                    return OperationResult<long>.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var categorySlug = await _unitOfWork.ProductCategoryRepository.GetSlugById(command.CategoryId);
                var path = $"{categorySlug}//{slug}";
                var picturePath = _fileUploader.Upload(command.Picture, path);
                var product = Product.Create(command.Name, command.Code,
                    command.ShortDescription, command.Description, picturePath,
                    command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                    command.Keywords, command.MetaDescription);
                await _unitOfWork.ProductRepository.Add(product);
                await _unitOfWork.CommitAsync();
                return OperationResult<long>.Succedded(product.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                        "#ProductApplication.Create.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<long>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<bool>> Edit(EditProduct command)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(command.Id);
                if (product == null)
                    return OperationResult<bool>.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.ProductRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                    return OperationResult<bool>.Failed(ApplicationMessages.DuplicatedRecord);

                var slug = command.Slug.Slugify();
                var path = $"{product.Category.Slug}/{slug}";

                var picturePath = _fileUploader.Upload(command.Picture, path);
                product.Edit(command.Name, command.Code,
                    command.ShortDescription, command.Description, picturePath,
                    command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                    command.Keywords, command.MetaDescription);
                await _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CommitAsync();
                return OperationResult<bool>.Succedded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.Edit.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<bool>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<EditProduct>> GetDetails(long id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(id);
                if (product == null)
                    return OperationResult<EditProduct>.Failed(ApplicationMessages.RecordNotFound);

                return OperationResult<EditProduct>.Succedded(new EditProduct
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
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<EditProduct>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<List<ProductViewModel>>> GetProducts()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsQueryable();
                if (products.Any() == false)
                    return OperationResult<List<ProductViewModel>>.Failed(ApplicationMessages.RecordNotFound);

                return OperationResult<List<ProductViewModel>>.Succedded(products.Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.GetDetails.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<List<ProductViewModel>>.Failed(e.Message);
            }
        }

        public async Task<OperationResult<List<ProductViewModel>>> Search(ProductSearchModel searchModel)
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
                    return OperationResult<List<ProductViewModel>>.Failed(ApplicationMessages.RecordNotFound);

                if (!string.IsNullOrWhiteSpace(searchModel.Name))
                    query = query.Where(x => x.Name.Contains(searchModel.Name));

                if (!string.IsNullOrWhiteSpace(searchModel.Code))
                    query = query.Where(x => x.Code.Contains(searchModel.Code));

                if (searchModel.CategoryId != 0)
                    query = query.Where(x => x.CategoryId == searchModel.CategoryId);

                return OperationResult<List<ProductViewModel>>.Succedded(query.Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category.Name,
                    CategoryId = x.CategoryId,
                    Code = x.Code,
                    Picture = x.Picture,
                    CreationDate = x.CreateDateTime.ToFarsi()
                }).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#ProductApplication.Search.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult<List<ProductViewModel>>.Failed(e.Message);
            }
        }
    }
}
