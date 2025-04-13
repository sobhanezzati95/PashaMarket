namespace Application.Dtos.ProductAggregate.Product
{
    public class ProductCategoryQueryModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public List<ProductQueryModel> ProductQueryModels { get; set; }
    }
    public class ProductQueryModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public double UnitPrice { get; set; }
        public string? Brand { get; set; }
        public double? UnitPriceAfterDiscount { get; set; }
        public int? DiscountPercentage { get; set; }
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
    }
}
