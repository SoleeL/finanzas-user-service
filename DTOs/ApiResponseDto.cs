namespace finanzas_user_service.Utilities;

public class ApiResponseDto<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    public ApiResponseDto()
    {
        Success = true;
    }
}