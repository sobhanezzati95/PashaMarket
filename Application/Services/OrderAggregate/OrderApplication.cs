using Application.Dtos.OrderAggregate;
using Application.Interfaces.OrderAggregate;
using Domain;
using Domain.Entities.OrderAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.OrderAggregate;
public class OrderApplication(IAuthenticationHelper authenticationHelper,
                              IConfiguration configuration,
                              IUnitOfWork unitOfWork,
                              ILogger<OrderApplication> logger)
    : IOrderApplication
{
    public async Task<long> PlaceOrder(Cart cart, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = authenticationHelper.CurrentUserId();
            var order = Order.PlaceOrder(currentUserId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
                                         cart.PayAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = OrderItem.AddItems(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                order.AddItem(orderItem);
            }

            await unitOfWork.OrderRepository.Add(order, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return order.Id;
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#OrderApplication.PlaceOrder.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<double> GetAmountBy(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await unitOfWork.OrderRepository.GetById(id, cancellationToken);
            return order != null ? order.PayAmount : 0;
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#OrderApplication.GetAmountsBy.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task Cancel(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await unitOfWork.OrderRepository.GetById(id, cancellationToken);
            order.Cancel();

            await unitOfWork.OrderRepository.Update(order, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#OrderApplication.Cancel.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<string> PaymentSucceeded(long orderId, long refId, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await unitOfWork.OrderRepository.GetById(orderId, cancellationToken);
            order.PaymentSucceeded(refId);
            var symbol = configuration.GetSection("Symbol").Value;
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            order.SetIssueTrackingNo(issueTrackingNo);
            //if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";
            await unitOfWork.OrderRepository.Update(order, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return issueTrackingNo;
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#OrderApplication.PaymentSucceeded.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<OrderItemViewModel>> GetItems(long orderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await unitOfWork.OrderRepository.GetAllWithIncludesAndThenInCludes(
                    predicate: x => x.Id == orderId,
                    orderBy: null,
                    isTracking: false,
                    ignoreQueryFilters: false,
                    includeProperties: null,
                    thenInclude: query => query.Include(x => x.Items).ThenInclude(p => p.Product));

            if (await order.AnyAsync(cancellationToken) == false)
                return [];

            var items = order.FirstOrDefault()!
                .Items
                .Select(x => new OrderItemViewModel
                {
                    Id = x.Id,
                    Count = x.Count,
                    DiscountRate = x.DiscountRate,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    UnitPrice = x.UnitPrice,
                    Product = x.Product.Name,
                }).ToList();
            return items;
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#OrderApplication.GetItems.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
    public async Task<List<OrderViewModel>> Search(OrderSearchModel searchModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = await unitOfWork.OrderRepository.GetAllWithIncludesAndThenInCludes(
                                    predicate: null,
                                    orderBy: x => x.OrderByDescending(x => x.Id),
                                    isTracking: false,
                                    ignoreQueryFilters: false,
                                    includeProperties: null,
                                    thenInclude: query => query.Include(x => x.User));

            if (await query.AnyAsync(cancellationToken) == false)
                return [];

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.UserId > 0)
                query = query.Where(x => x.UserId == searchModel.UserId);

            var orders = await query.Select(x => new OrderViewModel
            {
                Id = x.Id,
                UserId = x.UserId,
                DiscountAmount = x.DiscountAmount,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                CreationDate = x.CreateDateTime.ToFarsi(),
                UserFullName = x.User.Fullname,
                PaymentMethod = PaymentMethod.GetBy(x.PaymentMethod).Name
            }).ToListAsync();
            return orders;
        }
        catch (Exception e)
        {
            logger.LogError(e,
           "#OrderApplication.Search.CatchException() >> Exception: " + e.Message +
           (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
            throw;
        }
    }
}