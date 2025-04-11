namespace Application.Dtos.DiscountAggregate
{
    public class DiscountSearchModel
    {
        public long ProductId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
