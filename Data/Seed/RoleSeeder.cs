using finanzas_user_service.Data.Entities;
using finanzas_user_service.Enums;

namespace finanzas_user_service.Data.Seed;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (!context.Role.Any(r => r.Name == Roles.Admin.ToString()))
        {
            context.Role.Add(new Role 
            { 
                Id = (int) Roles.Admin, 
                Name = Roles.Admin.ToString()
            });
        }

        if (!context.Role.Any(r => r.Name == Roles.User.ToString()))
        {
            context.Role.Add(new Role 
            { 
                Id = (int) Roles.User, 
                Name = Roles.User.ToString()
            });
        }
        
        await context.SaveChangesAsync();
    }
}