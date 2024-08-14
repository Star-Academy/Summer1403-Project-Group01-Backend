using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs.Profile;

namespace Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProfileController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditProfileInfo([FromBody] EditProfileInfoDto editProfileInfoDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.Claims.First(x => x.Type == "UserId").Value;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized("User not found!");

        var passwordCheckResult = await _signInManager.CheckPasswordSignInAsync(user, editProfileInfoDto.OldPassword, false);
        if (!passwordCheckResult.Succeeded)
            return BadRequest("Incorrect old password!");

        if (user.UserName != editProfileInfoDto.UserName)
        {
            var existingUser = await _userManager.FindByNameAsync(editProfileInfoDto.UserName);
            if (existingUser != null)
                return BadRequest("Username is already reserved by another user!");
        }
        
        user.UserName = editProfileInfoDto.UserName;
        user.FirstName = editProfileInfoDto.FirstName;
        user.LastName = editProfileInfoDto.LastName;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return BadRequest(updateResult.Errors);

        var passwordChangeResult = await _userManager.ChangePasswordAsync(user, editProfileInfoDto.OldPassword, editProfileInfoDto.NewPassword);
        if (!passwordChangeResult.Succeeded)
            return BadRequest(passwordChangeResult.Errors);

        return Ok("Profile info updated successfully!");
    }
    
}