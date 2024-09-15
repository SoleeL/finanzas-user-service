namespace finanzas_user_service.DTOs;

public class GetUserDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string Fullname { get; set; }
    public string Role { get; set; }
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
}