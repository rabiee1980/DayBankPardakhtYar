using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Transfer.Core.Model.Base;
using Transfer.Core.Helpers;

namespace Transfer.Core.Extenstions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            try
            {
                app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/problem+json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            logger.LogError($"Something went wrong: {contextFeature.Error}");
                            context.Response.ContentType = "application/problem+json";
                            await context.Response.WriteAsync(new BaseDTO((int)HttpStatusCode.InternalServerError, "خطای ناشناخته رخ داده است", null).ToSerilizedString());
                        }
                    });
                });

                app.UseStatusCodePages(async context =>
                {
                    context.HttpContext.Response.ContentType = "application/problem+json";
                    int httpStatusCode = context.HttpContext.Response.StatusCode;
                    string errorMessage = "خطای ناشناخته رخ داده است";
                    switch ((HttpStatusCode)httpStatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            errorMessage = "آدرس مورد نظر یافت نشد";
                            break;
                        case HttpStatusCode.Unauthorized:
                            errorMessage = "دسترسی غیر مجاز";
                            break;
                        case HttpStatusCode.BadRequest:
                            errorMessage = "درخواست نامعتبر می باشد";
                            break;
                        case HttpStatusCode.Forbidden :
                            errorMessage = "شما دسترسی به این آدرس را ندارید";
                            break;

                    }
                    await context.HttpContext.Response.WriteAsync(new BaseDTO(httpStatusCode, errorMessage, null).ToSerilizedString());
                });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
    }
}
