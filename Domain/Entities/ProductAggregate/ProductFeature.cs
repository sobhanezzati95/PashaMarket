using Framework.Domain;

namespace Domain.Entities.ProductAggregate;
public class ProductFeature : BaseEntity<long>
{
    #region Constructor

    public ProductFeature(long id, string displayName, string value, long productId)
    {
        Id = id;
        DisplayName = displayName;
        Value = value;
        ProductId = productId;
    }

    protected ProductFeature(string displayName, string value, long productId)
    {
        DisplayName = displayName;
        Value = value;
        ProductId = productId;
    }

    #endregion

    #region Properties

    public string DisplayName { get; set; }
    public string Value { get; set; }
    public long ProductId { get; set; }

    #endregion

    #region Relations

    public virtual Product Product { get; set; }

    #endregion

    #region Behaviors

    public static ProductFeature Define(string displayName, string value, long productId)
        => new(displayName, value, productId);
    public void Edit(string displayName, string value, long productId)
    {
        DisplayName = displayName;
        Value = value;
        ProductId = productId;
    }

    #endregion
}