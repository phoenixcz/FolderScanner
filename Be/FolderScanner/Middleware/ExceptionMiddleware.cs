﻿using System;

namespace FolderScanner.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while processing request {TraceIdentifier}", context.TraceIdentifier);

            await HandleExceptionAsync(context, e).ConfigureAwait(false);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        var statusCode = e switch
        {
            DirectoryNotFoundException => 404,
            _ => 500
        };

        context.Response.ContentType = "text/plain";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(e.Message).ConfigureAwait(false);
    }
}