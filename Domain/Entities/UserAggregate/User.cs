using Domain.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserAggregate;
public class User : IdentityUser<long>
{
    #region Constructor
    protected User(string username, string? email)
    {
        UserName = username;
        Email = email;
    }
    public User()
    {

    }
    #endregion

    #region Properties

    public string? Fullname { get; private set; }
    public string? NationalCode { get; private set; }
    public DateTime? BirthDate { get; private set; }

    #endregion

    #region Relations

    public ICollection<Order> Orders { get; private set; }

    #endregion

    #region Behaviors

    public static User Register(string username, string? email)
        => new(username, email);
    public void Edit(string? fullname,
                     string userName,
                     string? phoneNumber,
                     string? nationalCode,
                     string? email,
                     DateTime? birthDate)
    {
        Fullname = fullname;
        UserName = userName;
        PhoneNumber = phoneNumber;
        NationalCode = nationalCode;
        Email = email;
        BirthDate = birthDate;
    }

    #endregion
}