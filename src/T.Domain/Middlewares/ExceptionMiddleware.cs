#region

using Microsoft.AspNetCore.Http;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Exceptions;

#endregion

namespace T.Domain.Middlewares;

public class ExceptionMiddleware : IMiddleware {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try { await next(context); }
        catch (ValidationException ex) { await HandleValidationException(context, ex); }
        catch (AppEx ex) { await HandleAppException(context, ex); }
        catch (UnauthorizedAccessException) { await HandleUnauthorizedAccessException(context); }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            await HandleException(context);
        }
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception ex) {
        var exception = (ValidationException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(exception.Errors));
    }

    private async Task HandleAppException(HttpContext httpContext, Exception ex) {
        var exception = (AppEx)ex;
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(exception.Message));
    }

    private async Task HandleException(HttpContext httpContext) {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(Messages.Error));
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext) {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await httpContext.Response.WriteAsJsonAsync(Result.Fail(Messages.User_NotAllowAccess));
    }
}
