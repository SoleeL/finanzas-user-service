using finanzas_user_service.Data.Entity;

namespace finanzas_user_service.Data.Seed;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (!context.Role.Any(r => r.Name == "Admin"))
        {
            context.Role.Add(new Role 
            { 
                Id = 1, 
                Name = "Admin"
            });
        }

        if (!context.Role.Any(r => r.Name == "User"))
        {
            context.Role.Add(new Role 
            { 
                Id = 2, 
                Name = "User"
            });
        }
        
        await context.SaveChangesAsync();
    }
}