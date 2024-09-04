using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Web.Startup;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        
        var roleName = Environment.GetEnvironmentVariable("ROOTUSER_ROLE")!;
        var userName = Environment.GetEnvironmentVariable("ROOTUSER_USERNAME")!;
        var email = Environment.GetEnvironmentVariable("ROOTUSER_EMAIL")!;
        var password = Environment.GetEnvironmentVariable("ROOTUSER_PASSWORD")!;
        
        var rootUser = await userManager.FindByNameAsync(userName);
        if (rootUser == null)
        {
            rootUser = new AppUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(rootUser, password);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to create root user");
            }
            
            var roleResult = await userManager.AddToRoleAsync(rootUser, roleName);
            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to add to role");
            }
        }
    }
}
