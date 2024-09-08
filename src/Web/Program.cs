using Web.Startup;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var config = builder.Configuration;

builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddInfrastructureServices();
builder.Services.ConfigureAppAuthenticationServices();
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCorsPolicy(config);

var app = builder.Build();
app.UseMiddlewareServices();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}
app.Run();