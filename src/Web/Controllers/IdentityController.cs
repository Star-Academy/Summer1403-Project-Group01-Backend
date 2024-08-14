using Application.DTOs.Identity;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Identity;
using Web.Identity;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Authorize]
    [RequiresClaim(Claims.Role, AppRoles.Admin)]
    public async Task<IActionResult> Signup([FromBody] SignupDto signupDto)
    {
        var result = await _identityService.SignUpUser(signupDto.ToCreateUserRequest());

        if (!result.Succeed)
        {
            return BadRequest(result.Message);
        }

        CreateUserResponse response = result.Value;
        
        return Ok(response.ToUserSignedUpDto());
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _identityService.Login(loginDto.ToLoginUserRequest());
        
        if (!result.Succeed)
        {
            return Unauthorized(result.Message);
        }
        
        LoginUserResponse response = result.Value;

        return Ok(response.ToUserLoggedInDto());
    }
}