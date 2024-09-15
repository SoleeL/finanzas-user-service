using System.ComponentModel.DataAnnotations.Schema;

namespace finanzas_user_service.Data.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public ICollection<User> Users { get; set; }
}