using finanzas_user_service.Data.Seed;

namespace finanzas_user_service.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await RoleSeeder.SeedRolesAsync(context);
    }
}