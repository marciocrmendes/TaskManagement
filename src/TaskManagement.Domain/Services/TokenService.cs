using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Services
{
    public sealed class TokenProvider(IConfiguration configuration)
    {
        public string Create(User user, AuthTypeEnum authTypeEnum)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(configuration["JWT:Secret"]!);

            var scopes = GetScopesForAuthType(authTypeEnum);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = configuration["JWT:Audience"],
                Issuer = configuration["JWT:Issuer"],
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Email, user.Email),
                    new("scope", string.Join(',', scopes)),
                ]),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static string GetScopesForAuthType(AuthTypeEnum authTypeEnum)
        {
            return authTypeEnum switch
            {
                AuthTypeEnum.User => "taskmanagement:user",
                AuthTypeEnum.Manager => "taskmanagement:user,taskmanagement:manager",
                _ => string.Empty
            };
        }
    }
}
