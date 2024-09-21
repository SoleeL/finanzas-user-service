using finanzas_user_service.Utilities;
using FluentValidation;
using FluentValidation.Results;

namespace finanzas_user_service.Filters;

public class ValidatorFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            return await next(context);
        }
        
        var dtoToBeValidated = context.Arguments.OfType<T>().FirstOrDefault();
        if (dtoToBeValidated is null)
        {
            var apiResponseDto = new ApiResponseDto<string>(); 
            apiResponseDto.Success = false;
            apiResponseDto.ErrorMessage = "The body is empty";
            return TypedResults.BadRequest(apiResponseDto);
        }
        
        var validationResult = await validator.ValidateAsync(dtoToBeValidated);
        if (!validationResult.IsValid)
        {
            var apiResponseDto = new ApiResponseDto<IDictionary<string,string[]>>(); 
            apiResponseDto.Success = false;
            apiResponseDto.Data = validationResult.ToDictionary();
            apiResponseDto.ErrorMessage = "Validation error";
            
            return TypedResults.BadRequest(apiResponseDto);
        }
        
        var results = await next(context);
        return results;
    }
}