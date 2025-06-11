using Application.Dtos.ProductAggregate.ProductCategory;
using Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ProductAggregate.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Code { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get; set; }

        public string? Brand { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public int Count { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Description { get; set; }

        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile? Picture { get; set; }

        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }

        [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string MetaDescription { get; set; }
        public List<ProductCategoryViewModel> Categories { get; set; }
    }
}
