namespace Application.Dtos.OrderAggregate
{
    public class OrderViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? UserFullName { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double PayAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCanceled { get; set; }
        public string IssueTrackingNo { get; set; }
        public long RefId { get; set; }
        public string CreationDate { get; set; }
    }
}