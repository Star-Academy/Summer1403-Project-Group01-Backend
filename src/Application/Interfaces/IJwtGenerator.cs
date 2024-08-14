using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(AppUser user, IList<string> roles);
}