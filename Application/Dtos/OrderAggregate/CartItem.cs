namespace Application.Dtos.OrderAggregate
{
    public class CartItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public double unitPriceAfterDiscount { get; set; }
        public string Picture { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public bool IsInStock { get; set; }
        public int DiscountRate { get; set; }
        public double Discount { get; set; }
        public double ItemPayAmount { get; set; }

        public CartItem()
        {
            TotalPrice = unitPriceAfterDiscount * Count;
        }

        public void CalculateTotalItemPrice()
        {
            TotalPrice = unitPriceAfterDiscount * Count;
        }
    }
}