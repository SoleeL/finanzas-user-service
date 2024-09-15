using finanzas_user_service.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace finanzas_user_service.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()") // Usa 'gen_random_uuid()' si usas PostgreSQL, 'NEWID()' para SQL Server
                .ValueGeneratedOnAdd();

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            
            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
            builder.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        });
        
        modelBuilder.Entity<Role>(builder =>
        {
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}