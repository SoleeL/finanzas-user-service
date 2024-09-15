using finanzas_user_service.Data.Entities;
using finanzas_user_service.DTOs;

namespace finanzas_user_service.Repositories;

public interface IUserRepository
{
    Task<bool> UserExist(string email);

    Task<string> RegisterUserAsync(User user);

    Task<User> GetUserByAuthenticatedTokenAsync(string token);

    Task<User?> GetUserByIdAsync(Guid guid);

    Task<List<User>> GetAllUsersAsync(
        PaginationDto paginationDto,
        string? email = null,
        string? nickname = null,
        string? fullname = null, 
        string? role = null
    );

    Task<User> UpdateUserByIdAsync(string id, User user);

    Task<User> DeletUserByIdAsync(string id);

    Task<string> ForgotPasswordUserAsync(string id);

    Task<User> ResetPasswordUserByTokenAsync(string token, string newPassword);

    Task<User> ConfirmUserByEmailAsync(string token, string email);
}