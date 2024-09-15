namespace finanzas_user_service.DTOs;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string Fullname { get; set; }
    public string HashedPassword { get; set; }
}