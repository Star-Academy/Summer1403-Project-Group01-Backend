using Application.Interfaces.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Identity;
using Web.Helper;
using Web.Identity;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("identity")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signup")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Signup([FromBody] SignupDto signupDto)
    {
        var result = await _userService.SignUpUser(signupDto.ToCreateUserRequest());

        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(Signup), result.Message));
        }

        var response = result.Value!;
        
        return Ok(response.ToUserSignedUpDto());
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _userService.Login(loginDto.ToLoginUserRequest());
        
        if (!result.Succeed)
        {
            return Unauthorized(Errors.New(nameof(Login), result.Message));
        }
        
        var response = result.Value!;

        return Ok(response.ToUserLoggedInDto());
    }
    
    [HttpPatch("change-role")]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto changeRoleDto)
    {
        var result = await _userService.ChangeRole(changeRoleDto.ToChangeRoleRequest());

        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(ChangeRole), result.Message));
        }

        return Ok("Role changed successfully!");
    }

    [HttpGet]
    [Authorize]
    [RequiresAnyRole(Claims.Role, AppRoles.Admin)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetUsersAsync()
    {
        var appUsersWithRoles = await _userService.GetUsersAsync();

        return Ok(appUsersWithRoles);
    }
}