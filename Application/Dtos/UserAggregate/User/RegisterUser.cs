using Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserAggregate.User;
public class RegisterUser
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Username { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [EmailAddress]
    public string Email { get; set; }
}
