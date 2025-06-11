using Application.Dtos.ProductAggregate.ProductPicture;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductPictureApplication> _logger;
        public ProductPictureApplication(IFileUploader fileUploader, IUnitOfWork unitOfWork, ILogger<ProductPictureApplication> logger)
        {
            _fileUploader = fileUploader;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateProductPicture command)
        {
            try
            {
                //if (_productPictureRepository.Exists(x => x.Picture == command.Picture && x.ProductId == command.ProductId))
                //    return operation.Failed(ApplicationMessages.DuplicatedRecord);

                var product = await _unitOfWork.ProductRepository.GetProductWithCategory(command.ProductId);

                var path = $"{product.Category.Slug}//{product.Slug}";
                var picturePath = await _fileUploader.Upload(command.Picture, path);

                var productPicture = ProductPicture.Create(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
                await _unitOfWork.ProductPictureRepository.Add(productPicture);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductPictureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Edit(EditProductPicture command)
        {
            try
            {
                var productPicture = await _unitOfWork.ProductPictureRepository.GetWithProductAndCategory(command.Id);
                if (productPicture == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                var path = $"{productPicture.Product.Category.Slug}//{productPicture.Product.Slug}";
                var picturePath = command.Picture != null
                    ? await _fileUploader.Upload(command.Picture, path)
                    : null;

                productPicture.Edit(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
                await _unitOfWork.ProductPictureRepository.Update(productPicture);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductPictureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<EditProductPicture> GetDetails(long id)
        {
            try
            {
                var productPicture = await _unitOfWork.ProductPictureRepository.GetById(id);

                return new EditProductPicture
                {
                    Id = productPicture.Id,
                    PictureAlt = productPicture.PictureAlt,
                    PictureTitle = productPicture.PictureTitle,
                    ProductId = productPicture.ProductId
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductPictureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Remove(long id)
        {
            try
            {
                var productPicture = await _unitOfWork.ProductPictureRepository.GetById(id);
                if (productPicture == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                productPicture.Remove();
                await _unitOfWork.ProductPictureRepository.Update(productPicture);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductPictureApplication.Remove.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<OperationResult> Restore(long id)
        {
            try
            {
                var productPicture = await _unitOfWork.ProductPictureRepository.GetById(id);
                if (productPicture == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                productPicture.Restore();
                await _unitOfWork.ProductPictureRepository.Update(productPicture);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductPictureApplication.Restore.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel)
        {
            try
            {
                var query = await _unitOfWork.ProductPictureRepository.GetAllWithIncludesAndThenInCludes(
                predicate: null,
                orderBy: x => x.OrderByDescending(p => p.Id),
                isTracking: false,
                ignoreQueryFilters: false,
                includeProperties: null,
                thenInclude: query => query.Include(x => x.Product));

                if (query.Any() == false)
                    return new();

                if (searchModel.ProductId != 0)
                    query = query.Where(x => x.ProductId == searchModel.ProductId);

                return query.Select(x => new ProductPictureViewModel
                {
                    Id = x.Id,
                    Product = x.Product.Name,
                    CreationDate = x.CreateDateTime.ToString(),
                    Picture = x.Picture,
                    ProductId = x.ProductId,
                    IsRemoved = x.IsRemoved
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductPictureApplication.Restore.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}