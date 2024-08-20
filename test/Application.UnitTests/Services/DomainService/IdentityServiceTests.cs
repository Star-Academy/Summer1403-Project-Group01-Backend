using Application.DTOs.Identity.ChangeRole;
using Application.DTOs.Identity.CreateUser;
using Application.DTOs.Identity.GetUser;
using Application.DTOs.Identity.LoginUser;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Mappers;
using Application.Services.DomainService;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Web.DTOs.Identity;
using Web.Mappers;

namespace test.Application.UnitTests.Services.DomainService;

public class IdentityServiceTests
{
    private readonly IUserManagerRepository _userManagerRepository;
    private readonly IRoleManagerRepository _roleManagerRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IdentityService _identityService;

    public IdentityServiceTests()
    {
        _userManagerRepository = Substitute.For<IUserManagerRepository>();
        _roleManagerRepository = Substitute.For<IRoleManagerRepository>();
        _jwtGenerator = Substitute.For<IJwtGenerator>();
        _identityService = new IdentityService(_userManagerRepository, _roleManagerRepository, _jwtGenerator);
    }

    // Signup Tests
    [Fact]
    public async Task SignUpUser_WhenRoleDoesNotExist_ReturnsFailResult()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            UserName = "MobinBarfi",
            Password = "Abc@123",
            Email = "mobinbr99@gmail.com",
            Role = "NonExistentRole"
        };

        _roleManagerRepository.RoleExistsAsync(createUserRequest.Role).Returns(Task.FromResult(false));

        // Act
        var result = await _identityService.SignUpUser(createUserRequest);

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal("role does not exist", result.Message);
    }
    
    [Fact]
    public async Task SignUpUser_WhenUserCreationFails_ReturnsFailResult()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            UserName = "MobinBarfi",
            Password = "Abc@123",
            Email = "mobinbr99@gmail.com",
            Role = "Admin"
        };

        _roleManagerRepository.RoleExistsAsync(createUserRequest.Role).Returns(Task.FromResult(true));

        _userManagerRepository.CreateAsync(Arg.Any<AppUser>(), createUserRequest.Password)
            .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "User creation failed" })));

        // Act
        var result = await _identityService.SignUpUser(createUserRequest);

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal("User creation failed", result.Message);
    }
    
    [Fact]
    public async Task SignUpUser_WhenRoleAssignmentFails_ReturnsFailResult()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            UserName = "MobinBarfi",
            Password = "Abc@123",
            Email = "mobinbr99@gmail.com",
            Role = "Admin"
        };

        _roleManagerRepository.RoleExistsAsync(createUserRequest.Role).Returns(Task.FromResult(true));

        _userManagerRepository.CreateAsync(Arg.Any<AppUser>(), createUserRequest.Password)
            .Returns(Task.FromResult(IdentityResult.Success));

        _userManagerRepository.SetRoleAsync(Arg.Any<AppUser>(), createUserRequest.Role)
            .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" })));

        // Act
        var result = await _identityService.SignUpUser(createUserRequest);

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal("Role assignment failed", result.Message);
    }

    [Fact]
    public async Task SignUpUser_WhenUserIsCreatedAndRoleAssignedSuccessfully_ReturnsSuccessResult()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            UserName = "MobinBarfi",
            Password = "Abc@123",
            Email = "mobinbr99@gmail.com",
            Role = "Admin"
        };

        var appUser = new AppUser { UserName = createUserRequest.UserName };
        var identityResultSuccess = IdentityResult.Success;

        _roleManagerRepository.RoleExistsAsync(createUserRequest.Role)
            .Returns(Task.FromResult(true));

        _userManagerRepository.CreateAsync(Arg.Is<AppUser>(u => u.UserName == createUserRequest.UserName), createUserRequest.Password)
            .Returns(Task.FromResult(identityResultSuccess));

        _userManagerRepository.SetRoleAsync(Arg.Is<AppUser>(u => u.UserName == createUserRequest.UserName), createUserRequest.Role)
            .Returns(Task.FromResult(identityResultSuccess));

        var expectedResponse = new CreateUserResponse
        {
            UserName = createUserRequest.UserName,
            Email = createUserRequest.Email,
            FirstName = "",
            LastName = "",
            Role = createUserRequest.Role
        };

        // Act
        var result = await _identityService.SignUpUser(createUserRequest);

        // Assert
        Assert.True(result.Succeed);
        Assert.NotNull(result.Value);

        var actualResponse = result.Value;

        Assert.Equal(expectedResponse.UserName, actualResponse.UserName);
        Assert.Equal(expectedResponse.Email, actualResponse.Email);
        Assert.Equal(expectedResponse.FirstName, actualResponse.FirstName);
        Assert.Equal(expectedResponse.LastName, actualResponse.LastName);
        Assert.Equal(expectedResponse.Role, actualResponse.Role);
    }
    
    // Login Tests
    [Fact]
    public async Task Login_WhenUsernameNotFound_ReturnsFailResult()
    {
        // Arrange
        var loginUserRequest = new LoginUserRequest
        {
            UserName = "NonExistentUser",
            Password = "Abc@123"
        };

        _userManagerRepository.FindByNameAsync(loginUserRequest.UserName).Returns(Task.FromResult<AppUser?>(null));

        // Act
        var result = await _identityService.Login(loginUserRequest);

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal("Invalid username/email!", result.Message);
    }
    
    [Fact]
    public async Task Login_WhenPasswordIncorrect_ReturnsFailResult()
    {
        // Arrange
        var loginUserRequest = new LoginUserRequest
        {
            UserName = "MobinBarfi",
            Password = "WrongPassword"
        };

        var appUser = new AppUser();
        _userManagerRepository.FindByNameAsync(loginUserRequest.UserName).Returns(Task.FromResult(appUser));
        _userManagerRepository.CheckPasswordAsync(appUser, loginUserRequest.Password).Returns(Task.FromResult(false));

        // Act
        var result = await _identityService.Login(loginUserRequest);

        // Assert
        Assert.False(result.Succeed);
        Assert.Equal("Username/Email not found and/or password incorrect", result.Message);
    }

    [Fact]
    public async Task Login_WhenLoginSucceeds_ReturnsSuccessResult()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UserName = "MobinBarfi",
            Password = "Abc@123"
        };

        var loginRequest = loginDto.ToLoginUserRequest();
        var appUser = new AppUser
        {
            UserName = loginDto.UserName,
            Email = "mobinbr99@gmail.com"
        };
        var role = "Admin";
        var token = "valid.jwt.token";

        var loginUserResponse = new LoginUserResponse
        {
            UserName = appUser.UserName,
            Email = appUser.Email,
            Token = token
        };

        // Set up the mock behavior
        _userManagerRepository.FindByNameAsync(loginRequest.UserName)
            .Returns(Task.FromResult(appUser));
        _userManagerRepository.CheckPasswordAsync(appUser, loginRequest.Password)
            .Returns(Task.FromResult(true));
        _userManagerRepository.GetRoleAsync(appUser)
            .Returns(Task.FromResult(role));
        _jwtGenerator.GenerateToken(appUser, role)
            .Returns(token);

        // Act
        var result = await _identityService.Login(loginRequest);

        // Assert
        Assert.True(result.Succeed);
        var loginResponse = result.Value;

        Assert.NotNull(loginResponse);
        Assert.Equal(loginUserResponse.UserName, loginResponse.UserName);
        Assert.Equal(loginUserResponse.Email, loginResponse.Email);
        Assert.Equal(loginUserResponse.Token, loginResponse.Token);
    }
}