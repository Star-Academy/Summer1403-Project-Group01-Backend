﻿using Application.DTOs.Identity;
using Web.DTOs.Identity;
using Web.DTOs.Profile;

namespace Web.Mappers;

public static class IdentityMapper
{
    public static CreateUserRequest ToCreateUserRequest(this SignupDto signupDto)
    {
        return new CreateUserRequest
        {
            FirstName = signupDto.FirstName,
            LastName = signupDto.LastName,
            Email = signupDto.Email,
            UserName = signupDto.UserName,
            Password = signupDto.Password,
            Role = signupDto.Role
        };
    }
    
    public static LoginUserRequest ToLoginUserRequest(this LoginDto loginDto)
    {
        return new LoginUserRequest
        {
            UserName = loginDto.UserName,
            Email = loginDto.Email,
            Password = loginDto.Password
        };
    }
    
    public static UserSignedUpDto ToUserSignedUpDto(this CreateUserResponse createUserResponse)
    {
        return new UserSignedUpDto
        {
            FirstName = createUserResponse.FirstName,
            LastName = createUserResponse.LastName,
            Email = createUserResponse.Email,
            UserName = createUserResponse.UserName,
            Role = createUserResponse.Role
        };
    }

    public static UserLoggedInDto ToUserLoggedInDto(this LoginUserResponse loginUserResponse)
    {
        return new UserLoggedInDto
        {
            UserName = loginUserResponse.UserName,
            Token = loginUserResponse.Token
        };
    }

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
}