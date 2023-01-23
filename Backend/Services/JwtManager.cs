using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiToDatabase.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _config;
        public JwtManager(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    //Subject = new ClaimsIdentity(new[] {
                    //    new Claim("Id", Guid.NewGuid().ToString()),
                    //    //new Claim("kid", )
                    //    //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    //    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    //}),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    Issuer = _config["Jwt:Issuer"],
                    Audience = _config["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
