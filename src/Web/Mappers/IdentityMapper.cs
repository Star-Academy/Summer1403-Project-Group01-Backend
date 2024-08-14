using Application.DTOs.Identity;
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
            FirstName = loginUserResponse.FirstName,
            LastName = loginUserResponse.LastName,
            Email = loginUserResponse.Email,
            UserName = loginUserResponse.UserName,
            Role = loginUserResponse.Role,
            Token = loginUserResponse.Token
        };
    }

    
}