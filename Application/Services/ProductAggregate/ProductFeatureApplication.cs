using Application.Dtos.ProductAggregate.ProductFeature;
using Application.Interfaces.ProductAggregate;
using Domain;
using Domain.Entities.ProductAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProductAggregate
{
    public class ProductFeatureApplication : IProductFeatureApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductFeatureApplication> _logger;

        public ProductFeatureApplication(IUnitOfWork unitOfWork, ILogger<ProductFeatureApplication> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperationResult> Create(CreateProductFeature command)
        {
            try
            {
                var query = await _unitOfWork.ProductFeatureRepository.GetAllWithIncludesAndThenInCludes(
                        predicate: x => x.ProductId == command.ProductId,
                        orderBy: null,
                        isTracking: false,
                        ignoreQueryFilters: false,
                        includeProperties: null,
                        thenInclude: null);

                var existFeature = query.ToList();

                if (existFeature.Any())
                    await _unitOfWork.ProductFeatureRepository.RemoveRange(existFeature);

                var productFeatures = new List<ProductFeature>();
                foreach (var item in command.Items)
                    productFeatures.Add(ProductFeature.Define(item.DisplayName, item.Value, command.ProductId));


                await _unitOfWork.ProductFeatureRepository.AddRange(productFeatures);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductFeatureApplication.Create.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<ProductFeatureViewModel>> GetDetails(long productId)
        {
            try
            {
                var query = await _unitOfWork.ProductFeatureRepository.GetAllWithIncludesAndThenInCludes(
                        predicate: x => x.ProductId == productId,
                        orderBy: null,
                        isTracking: false,
                        ignoreQueryFilters: false,
                        includeProperties: null,
                        thenInclude: x => x.Include(p => p.Product));

                if (!query.Any())
                    return new();

                return query.Select(x => new ProductFeatureViewModel
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Value = x.Value,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#ProductFeatureApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}