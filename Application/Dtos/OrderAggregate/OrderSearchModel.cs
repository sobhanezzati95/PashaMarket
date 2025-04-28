namespace Application.Dtos.OrderAggregate
{
    public class OrderSearchModel
    {
        public long UserId { get; set; }
        public bool IsCanceled { get; set; }
    }
}