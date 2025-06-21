using Framework.Domain;

namespace Domain.Entities.UserAggregate;
public class Role : BaseEntity<long>
{
    #region Constructor

    protected Role()
    {
    }

    protected Role(string name)
    {
        Name = name;
        Users = [];
    }

    public Role(long id, string name)
    {
        Id = id;
        Name = name;
    }
    #endregion

    #region Properties
    public string Name { get; private set; }

    #endregion

    #region Relations
    public List<User> Users { get; private set; }

    #endregion

    #region Behaviors

    public static Role Create(string name)
        => new Role(name);
    public void Edit(string name)
        => Name = name;

    #endregion
}