namespace ProtectedApi.Data.Entities;

public class Client
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string GrantType { get; set; }
    public string AllowedScopes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
