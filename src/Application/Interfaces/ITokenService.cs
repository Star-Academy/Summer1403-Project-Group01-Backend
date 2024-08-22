using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user, string role);
    Task<bool> IsTokenInvalidatedAsync(string token);
    Task AddInvalidatedTokenAsync(string token);
}