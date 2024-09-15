using finanzas_user_service.Data;
using finanzas_user_service.Data.Entities;
using finanzas_user_service.DTOs;
using finanzas_user_service.Enums;
using finanzas_user_service.Utilities;
using Microsoft.EntityFrameworkCore;

namespace finanzas_user_service.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly HttpContext _httpContext;

    public UserRepository(
        ApplicationDbContext applicationDbContext,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._dbContext = applicationDbContext;
        this._httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<bool> UserExist(string email)
    {
        return await this._dbContext.User.AnyAsync(u => u.Email == email);
    }

    public async Task<string> RegisterUserAsync(User user)
    {
        this._dbContext.User.Add(user);
        await this._dbContext.SaveChangesAsync();
        return user.Id.ToString();
    }

    public Task<User> GetUserByAuthenticatedTokenAsync(string token)
    {
        // README: usar .AsNoTracking()
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByIdAsync(Guid guid)
    {
        return await this._dbContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == guid);
    }

    public async Task<List<User>> GetAllUsersAsync(
        PaginationDto paginationDto,
        string? email = null,
        string? nickname = null,
        string? fullname = null,
        string? role = null
    )
    {
        var queryable = _dbContext.User.AsNoTracking().AsQueryable();
        
        if (!string.IsNullOrEmpty(email)) queryable = queryable.Where(u => u.Email.Contains(email));

        if (!string.IsNullOrEmpty(nickname)) queryable = queryable.Where(u => u.NickName.Contains(nickname));

        if (!string.IsNullOrEmpty(fullname)) queryable = queryable.Where(u => u.FullName.Contains(fullname));

        if (!string.IsNullOrEmpty(role) && Enum.TryParse<Roles>(role, out Roles roleEnum))
            queryable = queryable.Where(u => u.RoleId == (int)roleEnum);

        await _httpContext.PaginateAsync(queryable, paginationDto);
        return await queryable.PageBy(paginationDto).ToListAsync();
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