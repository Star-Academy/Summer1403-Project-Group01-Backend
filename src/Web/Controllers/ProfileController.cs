using Application.DTOs;
using Application.DTOs.Identity;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Profile;
using Web.Helper;
using Web.Identity;
using Web.Mappers;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditProfileInfo([FromBody] EditProfileInfoDto editProfileInfoDto)
    {
        var userId = User.Claims.First(x => x.Type == Claims.UserId).Value;

        Result result = await _profileService.EditProfileInfo(editProfileInfoDto.ToEditProfileInfoRequest(userId));

        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(EditProfileInfo), result.Message));
        }
        
        return Ok("Profile info updated successfully!");
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfileInfo()
    {
        var userId = User.Claims.First(x => x.Type == Claims.UserId).Value;

        var result = await _profileService.GetProfileInfo(new GetProfileInfoRequest{UserId = userId});

        if (!result.Succeed)
        {
            return NotFound(Errors.New(nameof(GetProfileInfo), "User not found!"));
        }
        
        var user = result.Value!;

        return Ok(user.ToProfileInfoDto());
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var userId = User.Claims.First(x => x.Type == Claims.UserId).Value;

        var result = await _profileService.ChangePassword(changePasswordDto.ToChangePasswordRequest(userId));
        
        if (!result.Succeed)
        {
            return BadRequest(Errors.New(nameof(ChangePassword), result.Message));
        }
        
        return Ok("Password changed successfully!");
    }
    
}