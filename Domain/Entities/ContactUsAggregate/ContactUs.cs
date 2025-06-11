using Framework.Domain;

namespace Domain.Entities.ContactUsAggregate;
public class ContactUs : BaseEntity<long>
{

    #region Constructor

    public ContactUs(string fullName, string phoneNumber, string email, string title, string description)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        Title = title;
        Description = description;
    }

    #endregion

    #region Properties

    public string FullName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    #endregion

    #region Behaviors

    public static ContactUs Create(string fullName, string phoneNumber, string email, string title, string description)
    {
        return new ContactUs(fullName, phoneNumber, email, title, description);
    }

    public void Edit(string fullName, string phoneNumber, string email, string title, string description)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        Title = title;
        Description = description;
    }

    #endregion

    #region Relations

    #endregion
}
