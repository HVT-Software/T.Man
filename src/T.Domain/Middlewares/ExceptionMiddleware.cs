using System.Net;
using Microsoft.AspNetCore.Http;
using T.Domain.Common;
using T.Domain.Extensions;

namespace T.Domain.Middlewares;

public class ExceptionMiddleware : IExceptionHandler {

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            await next(context);
        } catch (AppEx ex) {
            await HandleException(context, (int)HttpStatusCode.OK, ex);
        } catch (Exception ex) {
            await HandleException(context, (int)HttpStatusCode.BadRequest, ex);
        }
    }

    private static async Task HandleException(HttpContext context, int statusCode, Exception exception) {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(Result.Fail(exception.Message).ToString());
    }
}
