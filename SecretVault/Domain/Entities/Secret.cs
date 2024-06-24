namespace SecretVault.Domain.Entities
{
    public class Secret
    {
        public int Id { get; set; }
        public string Key { get; set; }=String.Empty;
        public string EncryptedValue { get; set; } = String.Empty;
    }
}
