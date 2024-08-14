using Application.DTOs;
using Application.DTOs.Identity;

namespace Application.Interfaces.Services;

public interface IProfileService
{
    Task<Result<EditProfileInfoResponse>> EditProfileInfo(EditProfileInfoRequest infoRequest);
}