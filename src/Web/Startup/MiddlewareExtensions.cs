using Web.Middleware;

namespace Web.Startup;

public static class MiddlewareExtensions
{
    public static WebApplication UseMiddlewareServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigins");
        
        app.UseMiddleware<TokenValidationMiddleware>();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        return app;
    }
}
