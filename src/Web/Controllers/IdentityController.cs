using Domain.Constants;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Identity;
using Web.Identity;
using Web.Interfaces;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtGenerator _jwtGeneratorService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityController(UserManager<AppUser> userManager,
        IJwtGenerator jwtGeneratorService,
        SignInManager<AppUser> signInManager, 
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _jwtGeneratorService = jwtGeneratorService;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Authorize]
    [RequiresClaim(Claims.Role, AppRoles.Admin)]
    public async Task<IActionResult> Signup([FromBody] CreateIdentityDto createIdentityDto)
    {
        if (!await _roleManager.RoleExistsAsync(createIdentityDto.Role))
        {
            return BadRequest("Role does not exist");
        }
        
        var appUser = new AppUser
        {
            FirstName = createIdentityDto.FirstName,
            LastName = createIdentityDto.LastName,
            Email = createIdentityDto.Email,
            UserName = createIdentityDto.Username
        };
        
        var appUserResult = await _userManager.CreateAsync(appUser, createIdentityDto.Password);
        if (!appUserResult.Succeeded)
        {
            return BadRequest(appUserResult.Errors);
        }
        
        var roleResult = await _userManager.AddToRoleAsync(appUser, createIdentityDto.Role);
        if (!roleResult.Succeeded)
        {
            return BadRequest(roleResult.Errors);
        }

        return Ok(new IdentityCreatedDto
        {
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email,
            Username = appUser.UserName,
            Role = createIdentityDto.Role.ToLower()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null) return Unauthorized("Invalid username!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
        
        var roles = await _userManager.GetRolesAsync(user);

        return Ok(
            new UserLoggedInDto
            {
                UserName = user.UserName,
                Token = _jwtGeneratorService.GenerateToken(user, roles)
            }
        );
    }
}