﻿using Application.DTOs.Identity;
using Domain.Entities;

namespace Application.Mappers;

public static class IdentityMapper
{
    public static AppUser ToAppUser(this CreateUserRequest createUserRequest)
    {
        return new AppUser
        {
            FirstName = createUserRequest.FirstName,
            LastName = createUserRequest.LastName,
            Email = createUserRequest.Email,
            UserName = createUserRequest.UserName
        };
    }

    public static CreateUserResponse ToCreateUserResponse(this AppUser appUser, string role)
    {
        return new CreateUserResponse
        {
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email,
            UserName = appUser.UserName,
            Role = role
        };
    }
    
    public static LoginUserResponse ToLoginUserResponse(this AppUser appUser, string role, string token)
    {
        return new LoginUserResponse
        {
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email,
            UserName = appUser.UserName,
            Role = role,
            Token = token
        };
    }
}