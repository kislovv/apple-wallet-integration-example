using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BL.Abstractions;
using BL.Configurations;
using BL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BL.Services;

public class JwtUtils(IOptionsMonitor<JwtOptions> jwtOptionsMonitor): IJwtUtils
{
    private readonly JwtOptions _jwtOptions = jwtOptionsMonitor.CurrentValue;
    
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            //TODO: вынести время жизни в конфиг и добавить вариант с refresh token
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}