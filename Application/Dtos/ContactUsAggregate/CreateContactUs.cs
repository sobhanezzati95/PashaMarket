using Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ContactUsAggregate
{
    public class CreateContactUs
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public required string FullName { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public required string Email { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public required string Title { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public required string Description { get; set; }
    }
}