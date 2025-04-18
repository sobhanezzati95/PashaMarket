using Framework.Domain;

namespace Domain.Entities.UserAggregate
{
    public class Role : BaseEntity<long>
    {
        #region Constructor
        protected Role()
        {
        }

        protected Role(string name)
        {
            Name = name;
            Users = new List<User>();
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
        {
            return new Role(name);
        }
        public void Edit(string name)
        {
            Name = name;
        }

        #endregion

    }
}