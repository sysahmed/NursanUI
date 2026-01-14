using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nursan.API.Services
{
    /// <summary>
    /// Сервис за работа с JWT токени
    /// </summary>
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
            _issuer = configuration["Jwt:Issuer"] ?? "NursanAPI";
            _audience = configuration["Jwt:Audience"] ?? "NursanClient";
            _expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        /// <summary>
        /// Генерира JWT токен за потребител
        /// </summary>
        public string GenerateToken(string username, string? userId = null, IList<string>? roles = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId ?? username),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, 
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                    ClaimValueTypes.Integer64)
            };

            // Добавяме роли ако има
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Валидира JWT токен
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

