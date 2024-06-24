namespace SecretVault.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public string Role { get; set; } = String.Empty; // "Admin" or "Operator"
    }
}
