﻿using System.Net;
using System.Text.Json;

namespace Bns.Api.Helpers;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            //response.ContentType = "application/json";

            //response.StatusCode = error switch
            //{
            //    KeyNotFoundException e => (int)HttpStatusCode.NotFound,// not found error
            //    _ => (int)HttpStatusCode.InternalServerError,// unhandled error
            //};
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}