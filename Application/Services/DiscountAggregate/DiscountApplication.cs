using Application.Dtos.DiscountAggregate;
using Application.Interfaces.DiscountAggregate;
using Domain;
using Domain.Entities.DiscountAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Application.Services.DiscountAggregate
{
    public class DiscountApplication : IDiscountApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DiscountApplication> _logger;

        public DiscountApplication(IUnitOfWork unitOfWork, ILogger<DiscountApplication> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Define(DefineDiscount command)
        {
            try
            {
                if (await _unitOfWork.DiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var startDate = command.StartDate.ToGeorgianDateTime();
                var endDate = command.EndDate.ToGeorgianDateTime();
                var customerDiscount = Discount.Create(command.ProductId, command.DiscountRate,
                    startDate, endDate, command.Reason);
                await _unitOfWork.DiscountRepository.Add(customerDiscount);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                        "#DiscountApplication.Define.CatchException() >> Exception: " + e.Message +
                        (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }

        public async Task<OperationResult> Edit(EditDiscount command)
        {
            try
            {
                var customerDiscount = await _unitOfWork.DiscountRepository.GetById(command.Id);

                if (customerDiscount == null)
                    return OperationResult.Failed(ApplicationMessages.RecordNotFound);

                if (await _unitOfWork.DiscountRepository.Exists(x => x.ProductId == command.ProductId
                && x.DiscountRate == command.DiscountRate && x.Id != command.Id))
                    return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

                var startDate = command.StartDate.ToGeorgianDateTime();
                var endDate = command.EndDate.ToGeorgianDateTime();
                customerDiscount.Edit(command.ProductId, command.DiscountRate, startDate, endDate, command.Reason);
                await _unitOfWork.DiscountRepository.Update(customerDiscount);
                await _unitOfWork.CommitAsync();
                return OperationResult.Succeeded();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#DiscountApplication.Edit.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                return OperationResult.Failed(e.Message);
            }
        }

        public async Task<EditDiscount> GetDetails(long id)
        {
            try
            {
                var discount = await _unitOfWork.DiscountRepository.GetById(id);
                return new EditDiscount
                {
                    Id = discount.Id,
                    ProductId = discount.ProductId,
                    DiscountRate = discount.DiscountRate,
                    StartDate = discount.StartDate.ToString(CultureInfo.InvariantCulture),
                    EndDate = discount.EndDate.ToString(CultureInfo.InvariantCulture),
                    Reason = discount.Reason
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#DiscountApplication.GetDetails.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<DiscountViewModel>> Search(DiscountSearchModel searchModel)
        {
            try
            {
                var query = (await _unitOfWork.DiscountRepository.GetAllWithIncludesAndThenInCludes(
                                predicate: null,
                                orderBy: x => x.OrderByDescending(p => p.Id),
                                isTracking: false,
                                ignoreQueryFilters: false,
                                includeProperties: null,
                                thenInclude: query => query.Include(x => x.Product)))
                                .Select(x => new DiscountViewModel
                                {
                                    Id = x.Id,
                                    DiscountRate = x.DiscountRate,
                                    EndDate = x.EndDate.ToFarsi(),
                                    EndDateGr = x.EndDate,
                                    StartDate = x.StartDate.ToFarsi(),
                                    StartDateGr = x.StartDate,
                                    ProductId = x.ProductId,
                                    Reason = x.Reason,
                                    CreationDate = x.CreateDateTime.ToFarsi(),
                                    Product = x.Product.Name,
                                });


                if (query.Any() == false)
                    return new();

                if (searchModel.ProductId > 0)
                    query = query.Where(x => x.ProductId == searchModel.ProductId);

                if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
                {
                    query = query.Where(x => x.StartDateGr > searchModel.StartDate.ToGeorgianDateTime());
                }

                if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
                {
                    query = query.Where(x => x.EndDateGr < searchModel.EndDate.ToGeorgianDateTime());
                }

                return query.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                "#DiscountApplication.Search.CatchException() >> Exception: " + e.Message +
                (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}
