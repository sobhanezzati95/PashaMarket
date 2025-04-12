using Domain.Entities.DiscountAggregate;
using Framework.Domain;

namespace Domain.Entities.ProductAggregate
{
    public class Product : BaseEntity<long>
    {
        #region Constructor

        public Product(long id, string name, string code, double unitPrice, int count, string shortDescription, string description, string picture,
            string pictureAlt, string pictureTitle, long categoryId, string slug, string keywords, string metaDescription)
        {
            Id = id;
            Name = name;
            Code = code;
            UnitPrice = unitPrice;
            Count = count;
            ShortDescription = shortDescription;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            CategoryId = categoryId;
            Slug = slug;
            Keywords = keywords;
            MetaDescription = metaDescription;
            IsInStock = true;
        }
        protected Product(string name, string code, double unitPrice, int count, string shortDescription, string description, string picture,
            string pictureAlt, string pictureTitle, long categoryId, string slug, string keywords, string metaDescription)
        {
            Name = name;
            Code = code;
            UnitPrice = unitPrice;
            Count = count;
            ShortDescription = shortDescription;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            CategoryId = categoryId;
            Slug = slug;
            Keywords = keywords;
            MetaDescription = metaDescription;
            IsInStock = true;
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public string Code { get; private set; }
        public double UnitPrice { get; private set; }
        public int Count { get; private set; }
        public bool IsInStock { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public long CategoryId { get; private set; }
        public string Slug { get; private set; }
        public string Keywords { get; private set; }
        public string MetaDescription { get; private set; }

        #endregion

        #region Relations

        public ProductCategory Category { get; private set; }
        public List<ProductPicture> ProductPictures { get; private set; }
        public List<Discount> Discounts { get; private set; }


        #endregion

        #region Behaviors

        public static Product Create(string name, string code, double unitPrice, int count, string shortDescription, string description, string picture,
            string pictureAlt, string pictureTitle, long categoryId, string slug, string keywords, string metaDescription)
        {
            return new Product(name, code, unitPrice, count, shortDescription, description, picture, pictureAlt, pictureTitle, categoryId, slug,
                keywords, metaDescription);
        }

        public void Edit(string name, string code, double unitPrice, int count, string shortDescription, string description, string picture,
            string pictureAlt, string pictureTitle, long categoryId, string slug, string keywords, string metaDescription)
        {
            Name = name;
            Code = code;
            UnitPrice = unitPrice;
            Count = count;
            ShortDescription = shortDescription;
            Description = description;

            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            CategoryId = categoryId;
            Slug = slug;
            Keywords = keywords;
            MetaDescription = metaDescription;
        }

        public void InStock()
        {
            IsInStock = true;
        }

        public void NotInStock()
        {
            IsInStock = false;
        }
        #endregion

    }
}
