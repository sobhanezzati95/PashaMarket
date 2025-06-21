using Application.Dtos.DiscountAggregate;
using Application.Interfaces.DiscountAggregate;
using Domain.Contracts;
using Domain.Entities.DiscountAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Application.Services.DiscountAggregate;
public class DiscountApplication(IUnitOfWork unitOfWork, ILogger<DiscountApplication> logger)
    : IDiscountApplication
{
    public async Task<OperationResult> Define(DefineDiscount command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await unitOfWork.DiscountRepository.Exists(x => x.ProductId == command.ProductId
                                                                && x.DiscountRate == command.DiscountRate, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            var customerDiscount = Discount.Create(command.ProductId, command.DiscountRate, startDate, endDate, command.Reason);
            await unitOfWork.DiscountRepository.Add(customerDiscount, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
                    "#DiscountApplication.Define.CatchException() >> Exception: " + e.Message +
                    (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            return OperationResult.Failed(e.Message);
        }
    }
    public async Task<OperationResult> Edit(EditDiscount command, CancellationToken cancellationToken = default)
    {
        try
        {
            var customerDiscount = await unitOfWork.DiscountRepository.GetById(command.Id, cancellationToken);
            if (customerDiscount == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);

            if (await unitOfWork.DiscountRepository.Exists(x => x.ProductId == command.ProductId
                                                                && x.DiscountRate == command.DiscountRate
                                                                && x.Id != command.Id, cancellationToken))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var startDate = command.StartDate.ToGeorgianDateTime();
            var endDate = command.EndDate.ToGeorgianDateTime();
            customerDiscount.Edit(command.ProductId, command.DiscountRate, startDate, endDate, command.Reason);
            await unitOfWork.DiscountRepository.Update(customerDiscount);
            await unitOfWork.CommitAsync(cancellationToken);
            return OperationResult.Succeeded();
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#DiscountApplication.Edit.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            return OperationResult.Failed(e.Message);
        }
    }
    public async Task<EditDiscount> GetDetails(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var discount = await unitOfWork.DiscountRepository.GetById(id, cancellationToken);
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
            logger.LogError(e,
            "#DiscountApplication.GetDetails.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<DiscountViewModel>> Search(DiscountSearchModel searchModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = (await unitOfWork.DiscountRepository.GetAllWithIncludesAndThenIncludes(
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

            if (await query.AnyAsync(cancellationToken) == false)
                return [];

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
                query = query.Where(x => x.StartDateGr > searchModel.StartDate.ToGeorgianDateTime());

            if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
                query = query.Where(x => x.EndDateGr < searchModel.EndDate.ToGeorgianDateTime());

            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#DiscountApplication.Search.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}