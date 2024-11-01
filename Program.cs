using finanzas_user_service.Data;
using finanzas_user_service.Endpoints;
using finanzas_user_service.Handlers;
using finanzas_user_service.Repositories;
using FluentValidation;
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

// Agregar servicios
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Usar cache
builder.Services.AddOutputCache();

// Agregar el Http context
builder.Services.AddHttpContextAccessor();

// Agregar automaper y su configuracion
builder.Services.AddAutoMapper(typeof(Program));

// Agregar servicio de validacion
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Agregar servicio de mensajes de error para el usuario
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Controlar excepciones
app.UseExceptionHandler();
app.UseStatusCodePages();

// Redireccion de request http a https
app.UseHttpsRedirection();

// Usar cache
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

app.MapGet("/error", () =>
{
    throw new ApplicationException("An unhandled exception occurred.");
});

app.MapGroup("/").MapUser();

app.Run();