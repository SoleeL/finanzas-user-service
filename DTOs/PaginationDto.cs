namespace finanzas_user_service.DTOs;

public class PaginationDto
{
    public int Page { get; set; } = 1;
    
    const int MaxSize = 50;
    private int _size = 10;
    public int Size
    {
        get
        {
            return _size;
        }
        set
        {
            _size = (value > MaxSize) ? MaxSize : value;
        }
    }
    
    // public int Total { get; set; }
    // public int TotalPages { get; set; }
}