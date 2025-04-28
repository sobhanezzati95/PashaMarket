using Application.Dtos.OrderAggregate;
using Application.Interfaces.OrderAggregate;
using Domain;
using Domain.Entities.OrderAggregate;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.OrderAggregate
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderApplication> _logger;
        public OrderApplication(IAuthenticationHelper authenticationHelper, IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<OrderApplication> logger)
        {
            _authenticationHelper = authenticationHelper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<long> PlaceOrder(Cart cart)
        {
            try
            {
                var currentUserId = _authenticationHelper.CurrentUserId();
                var order = Order.PlaceOrder(currentUserId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
                    cart.PayAmount);

                foreach (var cartItem in cart.Items)
                {
                    var orderItem = OrderItem.AddItems(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                    order.AddItem(orderItem);
                }

                await _unitOfWork.OrderRepository.Add(order);
                await _unitOfWork.CommitAsync();
                return order.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#OrderApplication.PlaceOrder.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<double> GetAmountBy(long id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(id);
                if (order != null)
                    return order.PayAmount;
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#OrderApplication.GetAmountsBy.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task Cancel(long id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(id);
                order.Cancel();

                await _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#OrderApplication.Cancel.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<string> PaymentSucceeded(long orderId, long refId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(orderId);
                order.PaymentSucceeded(refId);
                var symbol = _configuration.GetSection("Symbol").Value;
                var issueTrackingNo = CodeGenerator.Generate(symbol);
                order.SetIssueTrackingNo(issueTrackingNo);

                //if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

                await _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.CommitAsync();

                return issueTrackingNo;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#OrderApplication.PaymentSucceeded.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<OrderItemViewModel>> GetItems(long orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetAllWithIncludesAndThenInCludes(
                        predicate: x => x.Id == orderId,
                        orderBy: null,
                        isTracking: false,
                        ignoreQueryFilters: false,
                        includeProperties: null,
                        thenInclude: query => query.Include(x => x.Items).ThenInclude(p => p.Product));

                if (order.Any() == false)
                    return new();

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
                _logger.LogError(e,
               "#OrderApplication.GetItems.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }

        public async Task<List<OrderViewModel>> Search(OrderSearchModel searchModel)
        {
            try
            {
                var query = await _unitOfWork.OrderRepository.GetAllWithIncludesAndThenInCludes(
                                        predicate: null,
                                        orderBy: x => x.OrderByDescending(x => x.Id),
                                        isTracking: false,
                                        ignoreQueryFilters: false,
                                        includeProperties: null,
                                        thenInclude: query => query.Include(x => x.User));

                if (query.Any() == false)
                    return new();

                query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

                if (searchModel.UserId > 0) query = query.Where(x => x.UserId == searchModel.UserId);

                var orders = query.Select(x => new OrderViewModel
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
                }).ToList();

                return orders;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
               "#OrderApplication.Search.CatchException() >> Exception: " + e.Message +
               (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
                throw;
            }
        }
    }
}