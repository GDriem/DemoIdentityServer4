namespace ProtectedApi.Data.Entities;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserRole> UserRoles { get; set; }
}
