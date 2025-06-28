namespace Application.Dtos.UserAggregate.User
{
    public class EditUser
    {
        public long Id { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
