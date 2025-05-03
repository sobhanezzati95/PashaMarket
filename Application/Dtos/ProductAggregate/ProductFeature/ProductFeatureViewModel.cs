namespace Application.Dtos.ProductAggregate.ProductFeature
{
    public class ProductFeatureViewModel
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
    }
}