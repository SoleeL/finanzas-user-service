using finanzas_user_service.Data;
using finanzas_user_service.Data.Entities;
using finanzas_user_service.Enums;
using Microsoft.EntityFrameworkCore;

namespace finanzas_user_service.Repositories;

public class UserRepository : IUserRepository
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

    public Task<User> GetUserByAuthenticatedTokenAsync(string token)
    {
        // README: usar .AsNoTracking()
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByIdAsync(Guid guid)
    {
        return await this._context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == guid);
    }

    public async Task<List<User>> GetAllUsersAsync(
        string? email = null, 
        string? nickname = null, 
        string? fullname = null, 
        string? role = null
    )
    {
        var query = _context.User.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(email)) query = query.Where(u => u.Email.Contains(email));
        
        if (!string.IsNullOrEmpty(nickname)) query = query.Where(u => u.NickName.Contains(nickname));
        
        if (!string.IsNullOrEmpty(fullname)) query = query.Where(u => u.FullName.Contains(fullname));
        
        if (!string.IsNullOrEmpty(role) && Enum.TryParse<Roles>(role, out Roles roleEnum)) query = query.Where(u => u.RoleId == (int) roleEnum);
        
        return await query.ToListAsync();
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