namespace Framework.Domain;
public class BaseEntity<TKey>
{
    public TKey Id { get; protected set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreateDateTime { get; set; } = DateTime.UtcNow;
}