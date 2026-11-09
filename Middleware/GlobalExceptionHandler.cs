using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using task2.Exceptions;
using FluentValidation;

namespace task2.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, errors) = exception switch
        {
            AppException appEx => (appEx.StatusCode, appEx.Message, (IDictionary<string, string[]>?)null),

            ValidationException valEx => (400, "Validation failed", valEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),

            _ => (500, "Internal Server Error", null)
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = httpContext.Request.Path
        };

        if (errors != null)
        {
            problemDetails.Extensions.Add("errors", errors);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}