using Application.Dtos.ProductAggregate.ProductFeature;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate;
public class ProductFeatureApplication(IUnitOfWork unitOfWork, ILogger<ProductFeatureApplication> logger)
    : IProductFeatureApplication
{
    public async Task<OperationResult> Create(CreateProductFeature command, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = await unitOfWork.ProductFeatureRepository.GetAllWithIncludesAndThenInCludes(
                    predicate: x => x.ProductId == command.ProductId,
                    orderBy: null,
                    isTracking: false,
                    ignoreQueryFilters: false,
                    includeProperties: null,
                    thenInclude: null);

            var existFeature = await query.ToListAsync(cancellationToken);
            if (existFeature.Count != 0)
                await unitOfWork.ProductFeatureRepository.RemoveRange(existFeature, cancellationToken);

            var productFeatures = new List<ProductFeature>();
            foreach (var item in command.Items)
                productFeatures.Add(ProductFeature.Define(item.DisplayName, item.Value, command.ProductId));

            await unitOfWork.ProductFeatureRepository.AddRange(productFeatures, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductFeatureApplication.Create.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<ProductFeatureViewModel>> GetDetails(long productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = await unitOfWork.ProductFeatureRepository.GetAllWithIncludesAndThenInCludes(
                    predicate: x => x.ProductId == productId,
                    orderBy: null,
                    isTracking: false,
                    ignoreQueryFilters: false,
                    includeProperties: null,
                    thenInclude: x => x.Include(p => p.Product));

            if (!await query.AnyAsync(cancellationToken))
                return [];

            return await query
                .Select(x => new ProductFeatureViewModel
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Value = x.Value,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name
                }).ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#ProductFeatureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}