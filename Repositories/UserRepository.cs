using finanzas_user_service.Data;
using finanzas_user_service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace finanzas_user_service.Repositories;

public class UserRepository: IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<bool> UserExist(string email)
    {
        return await this._context.User.AnyAsync(u => u.Email == email);
    }

    public async Task<string> RegisterUserAsync(User user)
    {
        this._context.User.Add(user);
        await this._context.SaveChangesAsync();
        return user.Id.ToString();
    }

    public Task<User> GetAuthenticatedUserByTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByIdAsync(Guid guid)
    {
        return await this._context.User.FirstOrDefaultAsync(u => u.Id == guid);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await this._context.User.ToListAsync();
    }

    public Task<User> UpdateUserByIdAsync(string id, User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeletUserByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<string> ForgotPasswordUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<User> ResetPasswordUserByTokenAsync(string token, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<User> ConfirmUserByEmailAsync(string token, string email)
    {
        throw new NotImplementedException();
    }
}