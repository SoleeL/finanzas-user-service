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
            return TypedResults.BadRequest("The body is empty");
        }
        
        var validationResult = await validator.ValidateAsync(dtoToBeValidated);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        var results = await next(context);
        return results;
    }
}