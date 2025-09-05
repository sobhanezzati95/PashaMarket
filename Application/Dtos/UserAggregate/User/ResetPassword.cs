using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserAggregate.User;
public class ResetPassword
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
}