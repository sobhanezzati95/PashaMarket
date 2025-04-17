using Application.Dtos.ProductAggregate.ProductFeature;
using Application.Dtos.ProductAggregate.ProductPicture;

namespace Application.Dtos.ProductAggregate.Product
{
    public class ProductDetailQueryModel
    {
        public long Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public string Category { get; set; }
        public string CategorySlug { get; set; }
        public long CategoryId { get; set; }
        public double UnitPrice { get; set; }
        public bool IsInStock { get; set; }
        public string Keywords { get; set; }
        public string MetaDescription { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public List<ProductPictureViewModel> ProductPictures { get; set; }
        public List<ProductFeatureViewModel> ProductFeatures { get; set; }
    }
}
