using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserAggregate.User;
public class ForgetPassword
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}