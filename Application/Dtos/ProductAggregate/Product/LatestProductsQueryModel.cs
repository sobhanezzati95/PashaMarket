namespace Application.Dtos.ProductAggregate.Product
{
    public class LatestProductsQueryModel
    {
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public double UnitPriceAfterDiscount { get; set; }
        public int? DiscountPercentage { get; set; }
        public string Picture { get; set; }
    }
}
