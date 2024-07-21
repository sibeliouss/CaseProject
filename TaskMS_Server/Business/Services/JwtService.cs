using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.Concrete;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services;

public class JwtService
{
    public string CreateToken(User appUser, List<string?>? roles, bool rememberMe)
    {
        string token = string.Empty;

        List<Claim> claims =
        [
            new Claim("UserId", appUser.Id.ToString()),
            new Claim("Name", appUser.GetName()),
            new Claim("Email", appUser.Email ?? string.Empty),
            new Claim("UserName", appUser.UserName ?? string.Empty)
        ];
        if(roles is not null) claims.Add(new Claim("Roles", string.Join(",", roles)));

        DateTime expires = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: "Sibel Öztürk",
            audience: "Task Project",
            claims: claims,
            notBefore: DateTime.Now, 
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKeyStrongAndSecretKey")), SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();
        token = handler.WriteToken(jwtSecurityToken);

        return token;
    }
}