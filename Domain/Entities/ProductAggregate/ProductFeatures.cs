using Framework.Domain;

namespace Domain.Entities.ProductAggregate
{
    public class ProductFeatures : BaseEntity<long>
    {
        #region Constructor

        public ProductFeatures(long id, string displayName, string value, long productId)
        {
            Id = id;
            DisplayName = displayName;
            Value = value;
            ProductId = productId;
        }

        protected ProductFeatures(string displayName, string value, long productId)
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

        public static ProductFeatures Define(string displayName, string value, long productId)
        {
            return new ProductFeatures(displayName, value, productId);
        }

        public void Edit(string displayName, string value, long productId)
        {
            DisplayName = displayName;
            Value = value;
            ProductId = productId;
        }

        #endregion
    }
}
