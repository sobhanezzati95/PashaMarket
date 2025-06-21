using Framework.Domain;

namespace Domain.Entities.ProductAggregate;
public class ProductPicture : BaseEntity<long>
{
    #region Constructor

    public ProductPicture(long id, long productId, string picture, string pictureAlt, string pictureTitle)
    {
        Id = id;
        ProductId = productId;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
    }
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
        => new(productId, picture, pictureAlt, pictureTitle);
    public void Edit(long productId, string picture, string pictureAlt, string pictureTitle)
    {
        ProductId = productId;

        if (!string.IsNullOrWhiteSpace(picture))
            Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
    }

    #endregion
}