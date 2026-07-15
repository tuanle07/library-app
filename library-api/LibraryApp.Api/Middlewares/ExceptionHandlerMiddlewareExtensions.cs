using LibraryApp.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Middlewares;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception is null)
                {
                    return;
                }

                var statusCode = exception is AppException appException
                    ? appException.StatusCode
                    : StatusCodes.Status500InternalServerError;

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = statusCode == StatusCodes.Status500InternalServerError
                        ? "An unexpected error occurred."
                        : exception.Message
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            });
        });
    }
}
