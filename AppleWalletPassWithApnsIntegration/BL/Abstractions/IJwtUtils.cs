using BL.Entities;

namespace BL.Abstractions;

public interface IJwtUtils
{
    string GenerateToken(User user);
}