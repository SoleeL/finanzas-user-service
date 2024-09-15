using finanzas_user_service.DTOs;

namespace finanzas_user_service.Utilities;

public static class IQueryableExtensions
{
    public static IQueryable<T> PageBy<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
    {
        return queryable
            .Skip((paginationDto.Page - 1) * paginationDto.Size)
            .Take(paginationDto.Size);
    }
}