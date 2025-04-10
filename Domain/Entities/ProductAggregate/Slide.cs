using Framework.Domain;

namespace Domain.Entities.ProductAggregate
{
    public class Slide : BaseEntity<long>
    {
        #region Constructor

        public Slide(string picture, string pictureAlt, string pictureTitle, string heading,
             string title, string text, string link, string btnText)
        {
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Heading = heading;
            Title = title;
            Text = text;
            BtnText = btnText;
            Link = link;
            IsRemoved = false;
        }

        #endregion

        #region Properties

        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public string Heading { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string BtnText { get; private set; }
        public string Link { get; private set; }

        #endregion

        #region Behavior

        public static Slide Create(string picture, string pictureAlt, string pictureTitle, string heading,
             string title, string text, string link, string btnText)
        {
            return new Slide(picture, pictureAlt, pictureTitle, heading,
              title, text, link, btnText);
        }

        public void Edit(string picture, string pictureAlt, string pictureTitle, string heading,
                string title, string text, string link, string btnText)
        {
            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Heading = heading;
            Title = title;
            Text = text;
            BtnText = btnText;
            Link = link;
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

        #region Relations

        #endregion
    }
}
