using Application.Dtos.ProductAggregate.ProductCategory;
using Application.Interfaces.ProductAggregate;
using Domain.Contracts;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate;
public class ProductCategoryApplication(IUnitOfWork unitOfWork,
                                        IFileUploader fileUploader,
                                        ILogger<ProductCategoryApplication> logger)
    : IProductCategoryApplication
{
    public async Task<OperationResult> Create(CreateProductCategory command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await unitOfWork.ProductCategoryRepository.Exists(x => x.Name == command.Name, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var picturePath = $"{command.Slug}";
            var pictureName = await fileUploader.Upload(command.Picture, picturePath, cancellationToken);
            var productCategory = ProductCategory.Create(command.Name, command.Description, pictureName, command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, slug);
            await unitOfWork.ProductCategoryRepository.Add(productCategory, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
                    "#ProductCategoryApplication.Create.CatchException() >> Exception: " + e.Message +
                    (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            return OperationResult.Failed(e.Message);
        }
    }
    public async Task<OperationResult> Edit(EditProductCategory command, CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategory = await unitOfWork.ProductCategoryRepository.GetById(command.Id, cancellationToken);
            if (productCategory == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            if (await unitOfWork.ProductCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var picturePath = $"{command.Slug}";
            var fileName = command.Picture != null
                ? await fileUploader.Upload(command.Picture, picturePath, cancellationToken)
                : null;

            productCategory.Edit(command.Name, command.Description, fileName, command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, slug);
            await unitOfWork.ProductCategoryRepository.Update(productCategory);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#ProductCategoryApplication.Edit.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            return OperationResult.Failed(e.Message);
        }
    }
    public async Task<EditProductCategory> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategory = await unitOfWork.ProductCategoryRepository.GetById(id, cancellationToken);
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
            logger.LogError(e,
           "#ProductCategoryApplication.GetDetails.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<ProductCategoryViewModel>> GetProductCategories(CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategories = await unitOfWork.ProductCategoryRepository.GetAllAsQueryable();
            return await productCategories.Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#ProductCategoryApplication.GetProductCategories.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<ProductCategoryViewModel>> Search(ProductCategorySearchModel searchModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = await unitOfWork.ProductCategoryRepository.GetAllAsQueryable(cancellationToken);
            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            return await query
                .Select(x => new ProductCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    CreationDate = x.CreateDateTime.ToFarsi(),
                    Ispopular = x.IsPopular
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#ProductCategoryApplication.Search.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<MostPopularProductCategoriesQueryModel>> GetMostPopularProductCategoriesQuery(CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategories = await unitOfWork.ProductCategoryRepository.GetAllAsQueryable(cancellationToken);
            return await productCategories
                   .Where(x => x.IsPopular)
                   .Select(x => new MostPopularProductCategoriesQueryModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Picture = x.Picture,
                       PictureAlt = x.PictureAlt,
                       PictureTitle = x.PictureTitle,
                       Slug = x.Slug,
                   })
                   .AsNoTracking()
                   .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#ProductCategoryApplication.GetMostPopularProductCategoriesQuery.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<ProductCategoryQueryModel>> GetCategoriesQuery(CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategories = await unitOfWork.ProductCategoryRepository.GetAllAsQueryable(cancellationToken);
            return await productCategories
                .Select(x => new ProductCategoryQueryModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Keywords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#ProductCategoryApplication.GetCategoriesQuery.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> NotInStock(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategory = await unitOfWork.ProductCategoryRepository.GetById(id, cancellationToken);
            if (productCategory == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            productCategory.MakeItUnpopular();
            await unitOfWork.ProductCategoryRepository.Update(productCategory);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductCategoryApplication.NotInStock.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<OperationResult> InStock(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var productCategory = await unitOfWork.ProductCategoryRepository.GetById(id, cancellationToken);
            if (productCategory == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            var query = await unitOfWork.ProductCategoryRepository.GetAllAsQueryable(cancellationToken);
            var count = query.Where(x => x.IsPopular).Count();
            if (count >= 5)
                return OperationResult.Failed(ApplicationMessages.InvalidOperation);

            productCategory.MakeItPopular();
            await unitOfWork.ProductCategoryRepository.Update(productCategory);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#ProductCategoryApplication.InStock.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}