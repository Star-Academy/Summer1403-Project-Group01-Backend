using Application.DTOs;
using Application.DTOs.Identity;
using Application.DTOs.Identity.ChangeRole;

namespace Application.Interfaces.Services;

public interface IIdentityService
{
    Task<Result<CreateUserResponse>> SignUpUser(CreateUserRequest createIdentityDto);
    Task<Result<LoginUserResponse>> Login(LoginUserRequest loginDto);
    Task<Result> ChangeRole(ChangeRoleRequest changeRoleRequest);
}