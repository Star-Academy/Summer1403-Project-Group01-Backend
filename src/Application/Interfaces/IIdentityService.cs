﻿using Application.DTOs;
using Application.DTOs.Identity;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<Result<CreateUserResponse>> SignUpUser(CreateUserRequest createIdentityDto);
    Task<Result<LoginUserResponse>> Login(LoginUserRequest loginDto);
}