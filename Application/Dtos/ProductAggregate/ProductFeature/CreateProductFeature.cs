using Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ProductAggregate.ProductFeature
{
    public class CreateProductFeature
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public long ProductId { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public List<CreateProductFeatureItem> Items { get; set; }

    }
    public class CreateProductFeatureItem
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Value { get; set; }
    }
}
