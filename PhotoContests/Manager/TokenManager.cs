using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PhotoContests.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhotoContests.Manager
{
    public class TokenManager: ITokenManager
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public TokenManager(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public bool IsTokenExpired(string token)
        {
            var tokenManipulation = new JwtSecurityTokenHandler();
            var secret = configuration.GetSection("Jwt").GetSection("SecretKey").Get<string>();
            try
            {
                tokenManipulation.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var expirationDate = jwtToken.ValidTo;

                return expirationDate <= DateTime.UtcNow;
            }
            catch (Exception)
            {
                return true;
            }

        }

        public async Task<string> CreateToken(User user)
        {
            var secret = configuration.GetSection("Jwt").GetSection("SecretKey").Get<string>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDecription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenManipulation = new JwtSecurityTokenHandler();
            var token = tokenManipulation.CreateToken(tokenDecription); 

            return tokenManipulation.WriteToken(token);
        }
    }
}
