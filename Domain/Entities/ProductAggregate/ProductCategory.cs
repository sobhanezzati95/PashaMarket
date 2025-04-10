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

        protected ProductCategory(string name, string description, string picture, string pictureAlt, string pictureTitle, string keywords, string metaDescription, string slug)
        {
            Name = name;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Keywords = keywords;
            MetaDescription = metaDescription;
            Slug = slug;
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public string Keywords { get; private set; }
        public string MetaDescription { get; private set; }
        public string Slug { get; private set; }

        #endregion

        #region Relations

        public List<Product> Products { get; private set; }

        #endregion

        #region Behaviors

        public static ProductCategory Create(string name, string description, string picture, string pictureAlt, string pictureTitle, string keywords, string metaDescription, string slug)
        {
            return new ProductCategory(name, description, picture, pictureAlt, pictureTitle, keywords, metaDescription, slug);
        }

        public void Edit(string name, string description, string picture,
            string pictureAlt, string pictureTitle, string keywords, string metaDescription, string slug)
        {
            Name = name;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Keywords = keywords;
            MetaDescription = metaDescription;
            Slug = slug;
        }

        #endregion

    }
}
