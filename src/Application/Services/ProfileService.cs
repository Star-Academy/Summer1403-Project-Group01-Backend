using Application.DTOs;
using Application.DTOs.Identity;
using Application.ExtensionMethods;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappers;

namespace Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUserManager _userManager;

    public ProfileService(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<EditProfileInfoResponse>> EditProfileInfo(EditProfileInfoRequest infoRequest)
    {
        var user = await _userManager.FindByIdAsync(infoRequest.UserId);
        if (user == null)
            return Result<EditProfileInfoResponse>.Fail("User not found!");

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, infoRequest.OldPassword);
        if (!isPasswordCorrect)
            return Result<EditProfileInfoResponse>.Fail("Incorrect old password!");

        if (user.UserName != infoRequest.UserName)
        {
            var existingUser = await _userManager.FindByNameAsync(infoRequest.UserName);
            if (existingUser != null)
                return Result<EditProfileInfoResponse>.Fail("Username is already reserved by another user!");
        }
        
        user.UserName = infoRequest.UserName;
        user.FirstName = infoRequest.FirstName;
        user.LastName = infoRequest.LastName;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return Result<EditProfileInfoResponse>.Fail(updateResult.Errors.FirstMessage());

        var passwordChangeResult = await _userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
        if (!passwordChangeResult.Succeeded)
            return Result<EditProfileInfoResponse>.Fail(passwordChangeResult.Errors.FirstMessage());
        
        return Result<EditProfileInfoResponse>.Ok(new EditProfileInfoResponse()
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }

    public async Task<Result<GetProfileInfoResponse>> GetProfileInfo(GetProfileInfoRequest getProfileInfoRequest)
    {
        var user = await _userManager.FindByIdAsync(getProfileInfoRequest.UserId);
        
        if (user == null)
            return Result<GetProfileInfoResponse>.Fail("User not found!");

        var role = await _userManager.GetRoleAsync(user);
        
        return Result<GetProfileInfoResponse>.Ok(user.ToGetProfileInfoResponse(role));
    }
}