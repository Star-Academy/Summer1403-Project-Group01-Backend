﻿using System.Security.Claims;
using Application.DTOs;
using Application.DTOs.Identity;
using Application.DTOs.Identity.ChangeRole;
using Application.Interfaces.Services;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Web.Controllers;
using Web.DTOs.Identity;
using Web.Mappers;
using Web.Models;
using Xunit.Abstractions;

namespace test.Web.UnitTests.Controllers;

public class IdentityControllerTests
{
    private readonly IIdentityService _identityServiceMock;
    private readonly IdentityController _controller;

    public IdentityControllerTests()
    {
        _identityServiceMock = Substitute.For<IIdentityService>();
        _controller = new IdentityController(_identityServiceMock);
    }
    
    // Signup Tests
    
    // [Fact]
    public async Task Signup_WhenUserIsNotAdmin_ReturnsForbidden()
    {
        // Arrange
        var signupDto = new SignupDto
        {
            FirstName = "Mobin",
            LastName = "Barfi",
            Email = "mobinbr99@gmail.com",
            UserName = "MobinBarfi",
            Password = "Abc@1234",
            Role = "DataAnalyst"
        };

        _identityServiceMock.SignUpUser(Arg.Any<CreateUserRequest>()).Returns(Result<CreateUserResponse>.Ok(new CreateUserResponse()));
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Role, "DataAnalyst")
                }))
            }
        };

        // Act
        var result = await _controller.Signup(signupDto);

        // Assert
        Assert.IsType<ForbidResult>(result);
    }
    
    [Fact]
    public async Task Signup_WhenRoleDoesNotExist_ReturnsBadRequest()
    {
        // Arrange
        var signupDto = new SignupDto
        {
            FirstName = "Mobin",
            LastName = "Barfi",
            Email = "mobinbr99@gmail.com",
            UserName = "MobinBarfi",
            Password = "Abc@1234",
            Role = "NonExistentRole"
        };

        _identityServiceMock
            .SignUpUser(Arg.Any<CreateUserRequest>())
            .Returns(Result<CreateUserResponse>.Fail("role does not exist"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Role, "NonExistentRole")
                }))
            }
        };

        // Act
        var result = await _controller.Signup(signupDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);

        var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
        
        Assert.Equal("Signup", errorResponse.Title);
        Assert.NotNull(errorResponse.Message);
        Assert.Contains("role does not exist", errorResponse.Message);
    }
    
    [Fact]
    public async Task Signup_WhenSignUpSucceeds_ReturnsOkResult()
    {
        // Arrange
        var signupDto = new SignupDto
        {
            FirstName = "Mobin",
            LastName = "Barfi",
            Email = "mobinbr99@gmail.com",
            UserName = "MobinBarfi",
            Password = "Abc@1234",
            Role = "Admin"
        };

        var createUserResponse = new CreateUserResponse
        {
            FirstName = "Mobin",
            LastName = "Barfi",
            Email = "mobinbr99@gmail.com",
            UserName = "MobinBarfi",
            Role = "Admin"
        };

        _identityServiceMock
            .SignUpUser(Arg.Any<CreateUserRequest>())
            .Returns(Result<CreateUserResponse>.Ok(createUserResponse));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Role, AppRoles.Admin)
                }))
            }
        };

        // Act
        var result = await _controller.Signup(signupDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var responseValue = Assert.IsType<UserSignedUpDto>(okResult.Value);
        Assert.Equal("Mobin", responseValue.FirstName);
        Assert.Equal("Barfi", responseValue.LastName);
        Assert.Equal("mobinbr99@gmail.com", responseValue.Email);
        Assert.Equal("MobinBarfi", responseValue.UserName);
        Assert.Equal("Admin", responseValue.Role);
    }
    
    // Login Tests
    [Fact]
    public async Task Login_WhenLoginSucceeds_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UserName = "MobinBarfi",
            Password = "Abc@1234"
        };

        var mockResponse = new LoginUserResponse
        {
            UserName = "MobinBarfi",
            Token = "FakeToken"
        };

        _identityServiceMock
            .Login(Arg.Any<LoginUserRequest>())
            .Returns(Result<LoginUserResponse>.Ok(mockResponse));

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<UserLoggedInDto>(okResult.Value);
        Assert.Equal("MobinBarfi", response.UserName);
        Assert.Equal("FakeToken", response.Token);
    }
    
    // ChangeRole Tests
    [Fact]
    public async Task ChangeRole_WhenRoleDoesNotExist_ReturnsBadRequest()
    {
        // Arrange
        var changeRoleDto = new ChangeRoleDto
        {
            UserName = "MobinBarfi",
            Role = "NonExistentRole"
        };
        
        _identityServiceMock
            .ChangeRole(Arg.Any<ChangeRoleRequest>())
            .Returns(Result.Fail("role does not exist"));

        // Act
        var result = await _controller.ChangeRole(changeRoleDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);

        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("ChangeRole", errorResponse.Title);
        Assert.NotNull(errorResponse.Message);
        Assert.Contains("role does not exist", errorResponse.Message);
    }

    [Fact]
    public async Task ChangeRole_WhenOperationSucceeds_ReturnsOk()
    {
        // Arrange
        var changeRoleDto = new ChangeRoleDto
        {
            UserName = "MobinBarfi",
            Role = "Admin"
        };

        _identityServiceMock
            .ChangeRole(Arg.Any<ChangeRoleRequest>())
            .Returns(Result.Ok());

        // Act
        var result = await _controller.ChangeRole(changeRoleDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Role changed successfully!", okResult.Value);
    }
}