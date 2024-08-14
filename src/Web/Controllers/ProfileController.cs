using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Profile;
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

        if (result.Succeed)
        {
            return Ok("Profile info updated successfully!");
        }
        else
        {
            return BadRequest(result.Message);
        }
    }
    
}