using Framework.Domain;

namespace Domain.Entities.ProductAggregate
{
    public class ProductCategory : BaseEntity<long>
    {
        #region Constructor

        public ProductCategory()
        {
            Products = new List<Product>();
        }

        protected ProductCategory(string name, string description, string keywords, string metaDescription, string slug)
        {
            Name = name;
            Description = description;
            Keywords = keywords;
            MetaDescription = metaDescription;
            Slug = slug;
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Keywords { get; private set; }
        public string MetaDescription { get; private set; }
        public string Slug { get; private set; }

        #endregion

        #region Relations

        public List<Product> Products { get; private set; }

        #endregion

        #region Behaviors

        public static ProductCategory Create(string name, string description, string keywords, string metaDescription, string slug)
        {
            return new ProductCategory(name, description, keywords, metaDescription, slug);
        }

        public void Edit(string name, string description, string keywords, string metaDescription, string slug)
        {
            Name = name;
            Description = description;
            Keywords = keywords;
            MetaDescription = metaDescription;
            Slug = slug;
        }

        #endregion

    }
}
