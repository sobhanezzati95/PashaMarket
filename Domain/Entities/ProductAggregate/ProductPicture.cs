using Framework.Domain;

namespace Domain.Entities.ProductAggregate
{
    public class ProductPicture : BaseEntity<long>
    {
        #region Constructor

        protected ProductPicture(long productId, string picture, string pictureAlt, string pictureTitle)
        {
            ProductId = productId;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
        }

        #endregion

        #region Properties

        public long ProductId { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public Product Product { get; private set; }

        #endregion

        #region Relations

        #endregion

        #region Behaviors

        public static ProductPicture Create(long productId, string picture, string pictureAlt, string pictureTitle)
        {
            return new ProductPicture(productId, picture, pictureAlt, pictureTitle);
        }

        public void Edit(long productId, string picture, string pictureAlt, string pictureTitle)
        {
            ProductId = productId;

            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
        }

        public void Remove()
        {
            IsRemoved = true;
        }

        public void Restore()
        {
            IsRemoved = false;
        }

        #endregion

    }
}