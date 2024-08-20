using Application.DTOs;
using Application.DTOs.Profile.ChangePassword;
using Application.DTOs.Profile.EditProfile;
using Application.DTOs.Profile.GetProfileInfo;

namespace Application.Interfaces.Services;

public interface IProfileService
{
    Task<Result<EditProfileInfoResponse>> EditProfileInfo(EditProfileInfoRequest infoRequest);
    Task<Result<GetProfileInfoResponse>> GetProfileInfo(GetProfileInfoRequest profileRequest);
    Task<Result> ChangePassword(ChangePasswordRequest request);
}