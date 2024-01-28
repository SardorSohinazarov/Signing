namespace SigningAPI.Models
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public ICollection<int>? Roles { get; set; }
    }
}
