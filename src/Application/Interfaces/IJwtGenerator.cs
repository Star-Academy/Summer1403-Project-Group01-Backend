using Domain.Entities;

namespace Web.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(AppUser user, string role);
}