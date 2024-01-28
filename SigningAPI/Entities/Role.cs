namespace SigningAPI.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
