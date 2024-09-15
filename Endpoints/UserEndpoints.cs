using AutoMapper;
using finanzas_user_service.Data.Entities;
using finanzas_user_service.DTOs;
using finanzas_user_service.Enums;
using finanzas_user_service.Repositories;
using finanzas_user_service.Utilities;
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
        group.MapGet("/{guid:guid}", GetUserById)
            .CacheOutput(policyBuilder => policyBuilder.Expire(TimeSpan.FromSeconds(15)))
            .WithName("GetUser");

        // - GET /api/users
        // - Descripción: Lista todos los usuarios. Puede incluir parámetros para paginación y filtrado.
        // - Parámetros: Opcionalmente, page, size, sort, filter
        group.MapGet("/", GetAllUsers)
            .CacheOutput(policyBuilder => policyBuilder.Expire(TimeSpan.FromSeconds(15))) // Implementar cache
            .WithName("GetAllUsers")
            .WithOpenApi();

        return group;
    }

    static async Task<Results<BadRequest<string>, Created<GetUserDTO>>> RegisterUser(
        RegisterUserDTO registerUserDto,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        if (!MailRegexUtility.IsValidEmail(registerUserDto.Email))
        {
            return TypedResults.BadRequest("Invalid email");
        }

        var userExist = await userRepository.UserExist(registerUserDto.Email);

        if (userExist)
        {
            return TypedResults.BadRequest("Email already used");
        }
        
        var user = mapper.Map<User>(registerUserDto);
        
        await userRepository.RegisterUserAsync(user);
        
        var getUser = mapper.Map<GetUserDTO>(user);
        
        return TypedResults.Created($"/user/{user.Id}", getUser);
    }

    static async Task<Results<BadRequest<string>, NotFound, Ok<User>>> GetUserById(Guid guid,
        IUserRepository userRepository)
    {
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