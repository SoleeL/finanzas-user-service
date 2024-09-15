using System.Text.Json;
using finanzas_user_service.DTOs;
using Microsoft.EntityFrameworkCore;

namespace finanzas_user_service.Utilities;

public static class HttpContextExtensions
{
    public static async Task PaginateAsync<T>(
        this HttpContext httpContext,
        IQueryable<T> queryable,
        PaginationDto paginationDto
    )
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var totalItemCount = await queryable.CountAsync();
        var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)paginationDto.Size);

        // Determinar si hay una página siguiente y anterior
        var hasNext = paginationDto.Page < totalPageCount;
        var hasPrevious = paginationDto.Page > 1;

        // Construir URL de la página anterior y siguiente
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
        var previousPageUrl = hasPrevious ? $"{baseUrl}?page={paginationDto.Page - 1}&size={paginationDto.Size}" : null;
        var nextPageUrl = hasNext ? $"{baseUrl}?page={paginationDto.Page + 1}&size={paginationDto.Size}" : null;

        // Crear el objeto de paginación
        var xPaginationHeader = new
        {
            HasNext = hasNext,
            HasPrevious = hasPrevious,
            TotalPageCount = totalPageCount,
            TotalItemCount = totalItemCount,
            CurrentPage = paginationDto.Page,
            PageSize = paginationDto.Size,
            PreviousPageUrl = previousPageUrl,
            NextPageUrl = nextPageUrl
        };
        
        // Esta reemplazando & por \u0026
        httpContext.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(xPaginationHeader));
    }
}