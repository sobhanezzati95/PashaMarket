using Application.Dtos.ProductAggregate.ProductPicture;
using Application.Interfaces.ProductAggregate;
using Domain.Contracts;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate;
public class ProductPictureApplication(IFileUploader fileUploader,
                                       IUnitOfWork unitOfWork,
                                       ILogger<ProductPictureApplication> logger)
    : IProductPictureApplication
{
    public async Task<OperationResult> Create(CreateProductPicture command, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await unitOfWork.ProductRepository.GetProductWithCategory(command.ProductId, cancellationToken);
            var path = $"{product.Category.Slug}//{product.Slug}";
            var picturePath = await fileUploader.Upload(command.Picture, path, cancellationToken);
            var productPicture = ProductPicture.Create(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
            await unitOfWork.ProductPictureRepository.Add(productPicture, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductPictureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Edit(EditProductPicture command, CancellationToken cancellationToken = default)
    {
        try
        {
            var productPicture = await unitOfWork.ProductPictureRepository.GetWithProductAndCategory(command.Id, cancellationToken);
            var path = $"{productPicture.Product.Category.Slug}//{productPicture.Product.Slug}";
            var picturePath = command.Picture != null
                ? await fileUploader.Upload(command.Picture, path, cancellationToken)
                : null;

            productPicture.Edit(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
            await unitOfWork.ProductPictureRepository.Update(productPicture);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductPictureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<EditProductPicture> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var productPicture = await unitOfWork.ProductPictureRepository.GetById(id, cancellationToken);
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
            logger.LogError(e,
            "#ProductPictureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Remove(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var productPicture = await unitOfWork.ProductPictureRepository.GetById(id, cancellationToken);
            productPicture.Remove();
            await unitOfWork.ProductPictureRepository.Update(productPicture);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductPictureApplication.Remove.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> Restore(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var productPicture = await unitOfWork.ProductPictureRepository.GetById(id, cancellationToken);
            productPicture.Restore();
            await unitOfWork.ProductPictureRepository.Update(productPicture);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductPictureApplication.Restore.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<ProductPictureViewModel>> Search(ProductPictureSearchModel searchModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = await unitOfWork.ProductPictureRepository.GetAllWithIncludesAndThenIncludes(
            predicate: null,
            orderBy: x => x.OrderByDescending(p => p.Id),
            isTracking: false,
            ignoreQueryFilters: false,
            includeProperties: null,
            thenInclude: query => query.Include(x => x.Product));

            if (!await query.AnyAsync(cancellationToken))
                return [];

            if (searchModel.ProductId != 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            return await query
                .Select(x => new ProductPictureViewModel
                {
                    Id = x.Id,
                    Product = x.Product.Name,
                    CreationDate = x.CreateDateTime.ToString(),
                    Picture = x.Picture,
                    ProductId = x.ProductId,
                    IsActive = x.IsActive
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductPictureApplication.Restore.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}