using AutoMapper;
using finanzas_user_service.Data.Entities;
using finanzas_user_service.DTOs;
using finanzas_user_service.Filters;
using finanzas_user_service.Repositories;
using finanzas_user_service.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace finanzas_user_service.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUser(this RouteGroupBuilder group)
    {
        group.MapPost("/create", CreateUser)
            .AddEndpointFilter<ValidatorFilter<CreateUserDto>>()
            .CacheOutput(policyBuilder => policyBuilder.Expire(TimeSpan.FromSeconds(15))) // Implementar cache
            .WithName("CreateUser");

        // - GET /api/user/me
        // - Descripción: Obtiene la información del perfil del usuario autenticado. Utilizado para obtener información del perfil del usuario actual sin necesidad de especificar el userId.
        // TODO

        // - GET /api/user/{userId}
        // - Descripción: Obtiene la información del usuario especificado.
        // - Parámetros: userId (ID del usuario)

        // README: No es necesario implementar una validacion del guid, ya que ante uno no valido, el servicio
        // respondera automaticamente con 404

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

        // - PUT /api/user/{userId}
        // - Descripción: Actualiza la información del perfil del usuario.
        // - Cuerpo de Solicitud: Datos actualizados del usuario.
        // - Parámetros: userId (ID del usuario a actualizar)
        // TODO

        // - DELETE /api/user/{userId}
        // - Descripción: Elimina un usuario del sistema.
        // - Parámetros: userId (ID del usuario a eliminar)
        // TODO

        // - POST /api/user/forgot-password
        // - Descripción: Inicia el proceso de recuperación de contraseña. Puede enviar un enlace de recuperación al correo electrónico del usuario.
        // - Cuerpo de Solicitud: Correo electrónico del usuario
        // TODO

        // - POST /api/user/reset-password
        // - Descripción: Restablece la contraseña del usuario utilizando un token de recuperación.
        // - Cuerpo de Solicitud: Token de recuperación, nueva contraseña
        // TODO

        // - POST /api/user/confirm-email
        // - Descripción: Confirma la dirección de correo electrónico del usuario después del registro.
        // - Cuerpo de Solicitud: Token de confirmación
        // TODO

        return group;
    }

    private static async Task<Created<ApiResponseDto<GetUserDto>>> CreateUser(
        [FromBody] CreateUserDto createUserDto,
        IMapper mapper,
        IUserRepository userRepository
    )
    {
        var apiResponse = new ApiResponseDto<GetUserDto>();

        var user = mapper.Map<User>(createUserDto);

        await userRepository.RegisterUserAsync(user);

        var getUserDto = mapper.Map<GetUserDto>(user);

        apiResponse.Data = getUserDto;

        return TypedResults.Created($"/user/{apiResponse.Data.Id}", apiResponse);
    }

    private static async Task<Results<NotFound<ApiResponseDto<GetUserDto>>, Ok<ApiResponseDto<GetUserDto>>>>
        GetUserById(
            [FromRoute] Guid guid,
            IUserRepository userRepository,
            IMapper mapper
        )
    {
        var apiResponse = new ApiResponseDto<GetUserDto>();

        var user = await userRepository.GetUserByIdAsync(guid);

        if (user is null)
        {
            apiResponse.Success = false;
            apiResponse.ErrorMessage = "User not found";
            return TypedResults.NotFound(apiResponse);
        }

        var getUser = mapper.Map<GetUserDto>(user);
        apiResponse.Data = getUser;

        return TypedResults.Ok<ApiResponseDto<GetUserDto>>(apiResponse);
    }

    private static async Task<Ok<ApiResponseDto<List<GetUserDto>>>> GetAllUsers(
        IUserRepository userRepository,
        IMapper mapper,
        [FromQuery] string? email = null,
        [FromQuery] string? nickname = null,
        [FromQuery] string? fullname = null,
        [FromQuery] string? role = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10
    )
    {
        var apiResponse = new ApiResponseDto<List<GetUserDto>>();

        // TODO: Es necesario validar parameros FromUri
        // TODO: Es necesario validar parameros FromQuery
        var paginationDto = new PaginationDto() { Page = page, Size = size };

        // TODO: Es necesario validar parameros FromBody

        var users = await userRepository.GetAllUsersAsync(
            paginationDto,
            email,
            nickname,
            fullname,
            role);

        var getUsers = mapper.Map<List<GetUserDto>>(users);
        apiResponse.Data = getUsers;

        return TypedResults.Ok<ApiResponseDto<List<GetUserDto>>>(apiResponse);
    }
}