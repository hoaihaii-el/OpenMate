using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StaffManagmentNET.StaticServices
{
    public class JWTManager
    {
        private readonly IConfiguration _config;

        public JWTManager(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string userID, List<string> roles)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {
                new Claim("userID", userID + userID + userID),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:SecretKey"] ?? "")
                );

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authenKey,
                    SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
