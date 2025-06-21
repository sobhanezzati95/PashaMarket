namespace Application.Dtos.ProductAggregate.ProductPicture
{
    public class ProductPictureViewModel
    {
        public long Id { get; set; }
        public string Product { get; set; }
        public string Picture { get; set; }
        public string CreationDate { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
    }
}
