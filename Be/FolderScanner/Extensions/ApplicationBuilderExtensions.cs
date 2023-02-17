using FolderScanner.Middleware;

namespace FolderScanner.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}