namespace SigningAPI.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
