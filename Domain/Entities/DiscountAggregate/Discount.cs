using Domain.Entities.ProductAggregate;
using Framework.Domain;

namespace Domain.Entities.DiscountAggregate
{
    public class Discount : BaseEntity<long>
    {

        #region Constructor

        public Discount(long productId, int discountRate, DateTime startDate,
                  DateTime endDate, string reason)
        {
            ProductId = productId;
            DiscountRate = discountRate;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
        }

        #endregion

        #region Properties

        public long ProductId { get; private set; }
        public int DiscountRate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Reason { get; private set; }

        #endregion

        #region Behaviors

        public static Discount Create(long productId, int discountRate, DateTime startDate,
                  DateTime endDate, string reason)
        {
            return new Discount(productId, discountRate, startDate, endDate, reason);
        }

        public void Edit(long productId, int discountRate, DateTime startDate,
                  DateTime endDate, string reason)
        {
            ProductId = productId;
            DiscountRate = discountRate;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
        }

        #endregion

        #region Relations
        public Product Product { get; private set; }

        #endregion
    }
}
