using Application.DTOs;
using Application.DTOs.Identity;
using Application.Interfaces;
using Application.Interfaces.Services;

namespace Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUserManager _userManager;

    public ProfileService(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> EditProfileInfo(EditProfileInfoRequest infoRequest)
    {
        var user = await _userManager.FindByIdAsync(infoRequest.UserId);
        if (user == null)
            return Result.Fail("User not found!");

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, infoRequest.OldPassword);
        if (!isPasswordCorrect)
            return Result.Fail("Incorrect old password!");

        if (user.UserName != infoRequest.UserName)
        {
            var existingUser = await _userManager.FindByNameAsync(infoRequest.UserName);
            if (existingUser != null)
                return Result.Fail("Username is already reserved by another user!");
        }
        
        user.UserName = infoRequest.UserName;
        user.FirstName = infoRequest.FirstName;
        user.LastName = infoRequest.LastName;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return Result.Fail(updateResult.Errors.ToString());

        var passwordChangeResult = await _userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
        if (!passwordChangeResult.Succeeded)
            return Result.Fail(passwordChangeResult.Errors.ToString());
        
        return Result.Ok();
    }
}