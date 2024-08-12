using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Identity;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IdentityController(UserManager<AppUser> userManager) : ControllerBase
{
    public async Task<IActionResult> Register([FromBody] CreateIdentityDto createIdentityDto)
    {
        var appUser = new AppUser
        {
            UserName = createIdentityDto.Username
        };
        var appUserResult = await userManager.CreateAsync(appUser, createIdentityDto.Password);
        // if (appUserResult.Succeeded)
        // {
        //     
        // }
        // return Ok(new IdentityCreatedDto());
    }
}