namespace Application.Dtos.UserAggregate.User
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }
        public long? RoleId { get; set; }
        public string? Role { get; set; }
        public string? CreationDate { get; set; }
        public string? BirthDate { get; set; }
        public string? Email { get; set; }
    }
}
