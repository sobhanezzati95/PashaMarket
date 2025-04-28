using Domain.Entities.UserAggregate;
using Framework.Domain;

namespace Domain.Entities.OrderAggregate
{
    public class Order : BaseEntity<long>
    {
        #region Constructor

        protected Order(long userId, int paymentMethod, double totalAmount, double discountAmount, double payAmount)
        {
            UserId = userId;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
            PaymentMethod = paymentMethod;
            IsPaid = false;
            IsCanceled = false;
            RefId = 0;
            Items = new List<OrderItem>();
        }

        #endregion

        #region Properties

        public int PaymentMethod { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public double PayAmount { get; private set; }
        public bool IsPaid { get; private set; }
        public bool IsCanceled { get; private set; }
        public string IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }
        public long UserId { get; private set; }

        #endregion

        #region Relations

        public ICollection<OrderItem> Items { get; private set; }
        public User User { get; set; }

        #endregion

        #region Behaviors

        public static Order PlaceOrder(long userId, int paymentMethod, double totalAmount, double discountAmount, double payAmount)
        {
            return new Order(userId, paymentMethod, totalAmount, discountAmount, payAmount);
        }

        public void PaymentSucceeded(long refId)
        {
            IsPaid = true;

            if (refId != 0)
                RefId = refId;
        }

        public void Cancel()
        {
            IsCanceled = true;
        }

        public void SetIssueTrackingNo(string number)
        {
            IssueTrackingNo = number;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        #endregion
    }
}