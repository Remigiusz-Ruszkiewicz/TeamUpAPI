using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Data;
using TeamUpAPI.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace TeamUpAPI.Services
{
    public class TokenService : ITokenService
    {
        public TokenService(DataContext dbcontext)
        {
            Dbcontext = dbcontext;
        }
        public DataContext Dbcontext { get; }
        private const int ExpirationMinutes = 600;

        private static string CreateToken(User user)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                },
                expires: DateTime.UtcNow.AddMinutes(ExpirationMinutes),
                signingCredentials: CreateSigningCredentials()));
        }

        private static SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"])),
            SecurityAlgorithms.HmacSha256
            );
        }

        public async Task<AuthResponse?> LoginAsync(AuthRequest request)
        {
            var userInDb = Dbcontext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb is null)
                return null;
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, userInDb.Password);
            if (!isPasswordCorrect)
            {
                return null;
            }
            var accessToken = CreateToken(userInDb);
            await Dbcontext.SaveChangesAsync();
            return new AuthResponse
            {
                Username = userInDb.Username,
                Email = userInDb.Email,
                Token = accessToken,
            };
        }
        public async Task<AuthResponse?> RegisterAsync(AddUserRequest userRequest)
        {
            if (Dbcontext.Users.Any(x => x.Username == userRequest.Username))
                throw new Exception("Username '" + userRequest.Username + "' is already taken");
            if (Dbcontext.Users.Any(x => x.Email == userRequest.Email))
                throw new Exception("Email '" + userRequest.Email + "' is already used");

            userRequest.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            //string? friends = null;
            string? games = null;
            //if (userRequest.FriendsList != null)
            //{
            //    foreach (string friendId in userRequest.FriendsList)
            //    {
            //        if (friends != null)
            //        {
            //            if (!friends.Contains(friendId))
            //            {
            //                friends += $"{friendId};";
            //            }
            //        }
            //        else
            //        {
            //            friends = $"{friendId};";
            //        }
            //    }
            //}
            if (userRequest.GamesList != null)
            {
                foreach (string gameId in userRequest.GamesList)
                {
                    if (games != null)
                    {
                        if (!games.Contains(gameId))
                        {
                            games += $"{gameId};";
                        }
                    }
                    else
                    {
                        games = $"{gameId};";
                    }
                }
            }
            User user = new(
                    )
            { Id = Guid.NewGuid().ToString(), Username = userRequest.Username, StartHour = userRequest.StartHour, EndHour = userRequest.EndHour, Age = userRequest.Age, Email = userRequest.Email, Password = userRequest.Password, FriendsList = null, GamesList = games, };

            await Dbcontext.Users.AddAsync(user);
            await Dbcontext.SaveChangesAsync();
            var accessToken = CreateToken(user);
            return new AuthResponse
            {
                Username = user.Username,
                Email = user.Email,
                Token = accessToken,
            };
        }

    }
}
