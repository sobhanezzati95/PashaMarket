namespace Framework.Domain
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public bool IsActive { get; set; } = true;

        public Guid CreateUser { get; set; } = default!;
        public string CreateIp { get; set; } = default!;
        public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;

        public bool IsModified { get; set; } = false;
        public Guid? ModifyUser { get; set; }
        public string? ModifyIp { get; set; }
        public DateTime? ModifyDateTime { get; set; }

        public bool IsRemoved { get; set; } = false;
        public Guid? RemoveUser { get; set; }
        public string? RemoveIp { get; set; }
        public DateTime? RemoveDateTime { get; set; }
    }
}
