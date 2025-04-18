namespace Application.Dtos.UserAggregate.User
{
    public class ChangePassword
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
