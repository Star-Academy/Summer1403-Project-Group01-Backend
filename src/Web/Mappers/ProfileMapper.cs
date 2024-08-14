using Application.DTOs.Identity;
using Web.DTOs.Profile;

namespace Web.Mappers;

public static class ProfileMapper
{
    public static EditProfileInfoRequest ToEditProfileInfoRequest(this EditProfileInfoDto editProfileInfoDto,
        string userId)
    {
        return new EditProfileInfoRequest
        {
            UserId = userId,
            UserName = editProfileInfoDto.UserName,
            FirstName = editProfileInfoDto.FirstName,
            LastName = editProfileInfoDto.LastName,
            OldPassword = editProfileInfoDto.OldPassword,
            NewPassword = editProfileInfoDto.NewPassword
        };
    }
    
    public static ProfileInfoDto ToProfileInfoDto(this GetProfileInfoResponse getProfileInfoResponse)
    {
        return new ProfileInfoDto
        {
            FirstName = getProfileInfoResponse.FirstName,
            LastName = getProfileInfoResponse.LastName,
            Email = getProfileInfoResponse.Email,
            UserName = getProfileInfoResponse.UserName,
            Role = getProfileInfoResponse.Role
        };
    }
}