namespace SigningAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public string Salt { get; set; }

        public ICollection<Role>? Roles { get; set; }
    }
}
