using Application.Interfaces;

namespace Web.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public TokenValidationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Create a scope to resolve scoped services
        using var scope = _serviceProvider.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            if (await tokenService.IsTokenInvalidatedAsync(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}