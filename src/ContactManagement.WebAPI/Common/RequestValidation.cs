using FluentValidation;

namespace ContactManagement.WebAPI.Common;

public static class RequestValidation
{
    public static RouteHandlerBuilder WithFluentValidation<TRequest>(this RouteHandlerBuilder endpoint)
    {
        endpoint.AddEndpointFilter<RequestValidationFilter<TRequest>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        return endpoint;
    }
}

public class RequestValidationFilter<TRequest> : IEndpointFilter
{
    private readonly ILogger<RequestValidationFilter<TRequest>> _logger;
    private readonly IValidator<TRequest>? _validator;
    
    public RequestValidationFilter(ILogger<RequestValidationFilter<TRequest>> logger, IValidator<TRequest>? validator = null)
    {
        _logger = logger;
        _validator = validator;
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (_validator is null)
        {
            _logger.LogInformation("No validator found for type {Type}", typeof(TRequest).FullName);
            return await next(context);
        }

        var request = context.Arguments.OfType<TRequest>().First();
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Validation failed for type {Type} with following errors: {ValidationErrors}", 
                typeof(TRequest).FullName, validationResult.ToDictionary());
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        return await next(context);
    }
}