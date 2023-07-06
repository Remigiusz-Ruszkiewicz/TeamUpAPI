using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Data;
using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public class TokenService : ITokenService
    {
        public TokenService(DataContext dbcontext)
        {
            Dbcontext = dbcontext;
        }
        public DataContext Dbcontext { get; }
        private const int ExpirationMinutes = 30;

        private static string CreateToken(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private static JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",
                "apiWithAuthBackend",
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private static List<Claim> CreateClaims(User user)
        {
            try
            {
                return new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private static SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("!loremipsumzaq1@WSXzadupietojesttutenhashnaapidotokenajwt!")
                ),
                SecurityAlgorithms.HmacSha256
            );
        }

        public async Task<AuthResponse?> LoginAsync(AuthRequest request)
        {
            var userInDb = Dbcontext.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (userInDb is null)
                return null;
            var accessToken = CreateToken(userInDb);
            await Dbcontext.SaveChangesAsync();
            return new AuthResponse
            {
                Username = userInDb.Username,
                Email = userInDb.Email,
                Token = accessToken,
            };
        }
    }
}
