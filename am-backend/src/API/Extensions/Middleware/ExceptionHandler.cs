using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace AM.API.Extensions.Middleware;

internal static class ExceptionHandler
{
    public static void AddExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    string message = "Internal server error";
                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(new { context.Response.StatusCode, message })
                    );
                }
            });
        });
    }
}