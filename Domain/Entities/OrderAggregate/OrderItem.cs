using Domain.Entities.ProductAggregate;
using Framework.Domain;

namespace Domain.Entities.OrderAggregate;
public class OrderItem : BaseEntity<long>
{
    #region Constructor

    protected OrderItem(long productId,
                        int count,
                        double unitPrice,
                        int discountRate)
    {
        ProductId = productId;
        Count = count;
        UnitPrice = unitPrice;
        DiscountRate = discountRate;
    }

    #endregion

    #region Properties

    public int Count { get; private set; }
    public double UnitPrice { get; private set; }
    public int DiscountRate { get; private set; }
    public long ProductId { get; private set; }
    public long OrderId { get; private set; }

    #endregion

    #region Relations

    public Order Order { get; private set; }
    public Product Product { get; set; }

    #endregion

    #region Behaviors

    public static OrderItem AddItems(long productId, int count, double unitPrice, int discountRate)
        => new(productId, count, unitPrice, discountRate);

    #endregion
}