using finanzas_user_service.Data;
using finanzas_user_service.Data.Entity;
using finanzas_user_service.Endpoints;
using finanzas_user_service.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Definir DBs context
// Obtener variables de entorno
var host = Environment.GetEnvironmentVariable("USER_PG_HOST");
var database = Environment.GetEnvironmentVariable("USER_PG_DATABASE");
var user = Environment.GetEnvironmentVariable("USER_PG_USER");
var password = Environment.GetEnvironmentVariable("USER_PG_PASSWORD");

var connectionString = $"Host={host};Database={database};Username={user};Password={password}";
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString));

// Usar cache
builder.Services.AddOutputCache();


builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();

using var scope = app.Services.CreateScope();

// Ejecutar migraciones pendientes (NO RECOMENDADO):
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.MigrateAsync();

// Insertar filas base
await DatabaseInitializer.SeedAsync(dbContext);

// Leer el siguiente articulo sobre migraciones
// https://medium.com/geekculture/ways-to-run-entity-framework-migrations-in-asp-net-core-6-37719993ddcb

// Pipeline con Azure:
// https://anuraj.dev/blog/run-ef-core-migrations-in-azure-devops/

// Migracion automatica:
// https://www.youtube.com/watch?v=o9eEEFGgSHw

app.MapGroup("/").MapUser();

app.Run();