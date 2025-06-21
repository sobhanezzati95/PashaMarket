using Domain.Entities.OrderAggregate;
using Framework.Domain;

namespace Domain.Entities.UserAggregate;
public class User : BaseEntity<long>
{
    #region Constructor

    protected User(long id,
                   string? fullname,
                   string username,
                   string? mobile,
                   string? email,
                   string password,
                   DateTime? birthDate,
                   long roleId)
    {
        Id = id;
        Fullname = fullname;
        Username = username;
        Mobile = mobile;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        RoleId = roleId;
    }

    protected User(string username, string? email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
        RoleId = 2;
    }

    #endregion

    #region Properties

    public string? Fullname { get; private set; }
    public string Username { get; private set; }
    public string? Mobile { get; private set; }
    public string? Email { get; private set; }
    public string Password { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public long RoleId { get; private set; }

    #endregion

    #region Relations

    public ICollection<Order> Orders { get; private set; }
    public Role Role { get; private set; }

    #endregion

    #region Behaviors

    public static User Create(long id,
                              string? fullname,
                              string username,
                              string? mobile,
                              string? email,
                              string password,
                              DateTime? birthDate,
                              long roleId)
        => new(id,
               fullname,
               username,
               mobile,
               email,
               password,
               birthDate,
               roleId);

    public static User Register(string username, string? email, string password)
        => new(username, email, password);

    public void Edit(string? fullname,
                     string username,
                     string? mobile,
                     string? email,
                     DateTime? birthDate)
    {
        Fullname = fullname;
        Username = username;
        Mobile = mobile;
        Email = email;
        BirthDate = birthDate;
    }

    public void ChangePassword(string password)
        => Password = password;

    #endregion
}