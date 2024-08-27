using Application.DTOs;
using Application.DTOs.Identity.ChangeRole;
using Application.DTOs.Identity.CreateUser;
using Application.DTOs.Identity.GetUser;
using Application.DTOs.Identity.LoginUser;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<CreateUserResponse>> SignUpUser(CreateUserRequest createIdentityDto);
    Task<Result<LoginUserResponse>> Login(LoginUserRequest loginDto);
    Task<Result> ChangeRole(ChangeRoleRequest changeRoleRequest);
    Task<List<GetUserResponse>> GetUsersAsync();
}