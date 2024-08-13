using Infrastructure.Entities;

namespace Web.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(AppUser user);
}