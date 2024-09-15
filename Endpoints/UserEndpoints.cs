using finanzas_user_service.Data.Entity;
using finanzas_user_service.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace finanzas_user_service.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUser(this RouteGroupBuilder group)
    {
        group.MapPost("/register", RegisterUser)
            .CacheOutput(policyBuilder => policyBuilder.Expire(TimeSpan.FromSeconds(15))) // Implementar cache
            .WithName("RegisterUser");
        
        // - GET /api/user/{userId}
        // - Descripción: Obtiene la información del usuario especificado.
        // - Parámetros: userId (ID del usuario)
        group.MapGet("/{id}", GetUserById)
            .CacheOutput(policyBuilder => policyBuilder.Expire(TimeSpan.FromSeconds(15)))
            .WithName("GetUser");

        // - GET /api/users
        // - Descripción: Lista todos los usuarios. Puede incluir parámetros para paginación y filtrado.
        // - Parámetros: Opcionalmente, page, size, sort, filter
        group.MapGet("/", GetAllUsers)
            // .CacheOutput(policyBuilder => policyBuilder.Expire(TimeSpan.FromSeconds(15))) // Implementar cache
            .WithName("GetAllUsers")
            .WithOpenApi();

        return group;
    }
    
    static async Task<Created<string>> RegisterUser(User user, IUserRepository userRepository)
    {
        var id = await userRepository.RegisterUserAsync(user);
        return TypedResults.Created($"/user/{id}", id);
    }
    
    static async Task<Results<BadRequest<string>, NotFound, Ok<User>>> GetUserById(string id, IUserRepository userRepository)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return TypedResults.BadRequest("El id proporcionado no tiene un formato válido.");
        }
        
        var user = await userRepository.GetUserByIdAsync(guid);

        if (user is null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok<User>(user);
    }
    
    static async Task<Ok<List<User>>> GetAllUsers(IUserRepository userRepository)
    {
        var users = await userRepository.GetAllUsersAsync();
        return TypedResults.Ok<List<User>>(users);
    }
}