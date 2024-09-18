using finanzas_user_service.DTOs;
using finanzas_user_service.Repositories;
using FluentValidation;

namespace finanzas_user_service.Validators;

public class CreateUserDtoValidator: AbstractValidator<CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(createUserDto => createUserDto.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(UserExistValidation).WithMessage("Email already used");
        
        RuleFor(createUserDto => createUserDto.Nickname)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(32);
        
        RuleFor(createUserDto => createUserDto.Fullname)
            .NotEmpty()
            .MinimumLength(16)
            .MaximumLength(64);
        
        RuleFor(createUserDto => createUserDto.HashedPassword)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(64);
    }
    
    private async Task<bool> UserExistValidation(string email, CancellationToken _)
    {
        var userExist = await _userRepository.UserExist(email);
        return !userExist;
    }
}