using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserAggregate.User;
public class ChangePassword
{
    public long Id { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
    [Compare(nameof(NewPassword))]
    public required string RePassword { get; set; }
}
