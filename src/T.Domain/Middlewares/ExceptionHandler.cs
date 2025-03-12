using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Exceptions;
using T.Domain.Extensions;

namespace T.Domain.Middlewares;

public class ExceptionHandler : IExceptionHandler {
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> exceptionHandlers;

    public ExceptionHandler() {
        exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>> {
            { typeof(Exception), HandleException },
            { typeof(AppEx), HandleAppException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ValidationException), HandleValidationException }
        };
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
        var exceptionType = exception.GetType();

        if (!exceptionHandlers.TryGetValue(exceptionType, out var handler)) {
            return false;
        }
        await handler.Invoke(httpContext, exception);
        return true;

    }

    private async Task HandleValidationException(HttpContext httpContext, Exception ex) {
        var exception = (ValidationException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(exception.Errors));
    }

    private async Task HandleAppException(HttpContext httpContext, Exception ex) {
        var exception = (AppEx)ex;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(exception.Message));
    }

    private async Task HandleException(HttpContext httpContext, Exception ex) {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(Messages.Error));
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex) {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(Messages.User_NotAllowAccess));
    }
}
