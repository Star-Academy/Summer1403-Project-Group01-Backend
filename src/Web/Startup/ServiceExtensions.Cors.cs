using Microsoft.Extensions.DependencyInjection;

namespace Web.Startup
{
    public static partial class ServiceExtensions
    {
        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration config)
        {
            var origins = config.GetSection("CorsSettings:Origins").Get<string[]>()!;
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}