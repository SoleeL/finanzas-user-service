using System.ComponentModel.DataAnnotations.Schema;

namespace finanzas_user_service.Data.Entity;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column(TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<User> Users { get; set; }
}