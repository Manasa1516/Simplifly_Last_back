using Microsoft.IdentityModel.Tokens;
using Simplifly.Interfaces;
using Simplifly.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simplifly.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _keyString;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public TokenService(IConfiguration configuration)
        {
            _keyString = configuration["SecretKey"].ToString();
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyString));
        }

        /// <summary>
        /// Method to generate token
        /// </summary>
        /// <param name="user">Object of LoginUserDTO</param>
        /// <returns>Generated token in string</returns>
        public async Task<string> GenerateToken(LoginUserDTO user)
        {
            string token = string.Empty;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var cred = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            token = tokenHandler.WriteToken(myToken);
            return token;
        }
    }
}