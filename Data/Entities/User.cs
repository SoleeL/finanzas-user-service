namespace finanzas_user_service.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public string FullName { get; set; }
    public string HashedPassword { get; set; }
    public int RoleId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public Role Role { get; set; }
}