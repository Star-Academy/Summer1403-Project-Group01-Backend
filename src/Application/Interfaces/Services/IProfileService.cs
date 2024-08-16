using Application.DTOs;
using Application.DTOs.Identity;
using Application.DTOs.Profile.ChangePassword;

namespace Application.Interfaces.Services;

public interface IProfileService
{
    Task<Result<EditProfileInfoResponse>> EditProfileInfo(EditProfileInfoRequest infoRequest);
    Task<Result<GetProfileInfoResponse>> GetProfileInfo(GetProfileInfoRequest profileRequest);
    Task<Result> ChangePassword(ChangePasswordRequest request);
}