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
using NLog;

namespace TeamUpAPI.Services
{
    public class AuthService : IAuthService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public AuthService(DataContext dbcontext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            Dbcontext = dbcontext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public DataContext Dbcontext { get; }
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private const int ExpirationMinutes = 600;

        private static string CreateToken(User? user)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user?.Id),
                    new Claim(ClaimTypes.Name, user?.UserName),
                    new Claim(ClaimTypes.Email, user?.Email)
                },
                expires: DateTime.UtcNow.AddMinutes(ExpirationMinutes)
               ));
        }

        public async Task<AuthOperationResponse> LoginAsync(AuthRequest authRequest)
        {
            var user = await _userManager.FindByEmailAsync(authRequest.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, authRequest.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, authRequest.Password, false, false);
                    if (result.Succeeded)
                    {
                        return new AuthOperationResponse()
                        {
                            Result = true,
                        };
                    }
                    else
                    {
                        _logger.Info("Login Failed");
                        return new AuthOperationResponse()
                        {
                            Result = false,
                            Errors = new List<string>() { "Login Failed" }
                        };
                    }
                }
                else
                {
                    _logger.Info("Password is incorrect");
                    return new AuthOperationResponse()
                    {
                        Result = false,
                        Errors = new List<string>() { "Password is incorrect" }
                    };
                }
            }
            else
            {
                _logger.Info("Can not find user with this mail");
                return new AuthOperationResponse()
                {
                    Result = false,
                    Errors = new List<string>() { "Can not find user with this mail" }
                };
            }
        }
        public async Task<AuthOperationResponse> RegisterAsync(AddUserRequest userRequest)
        {
            var user = new User
            {
                UserName = userRequest.Username,
                Email = userRequest.Email,
                GamesList = userRequest.GamesList,
                FriendsList = userRequest.FriendsList,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userRequest.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User could not be created");
            }
            return new AuthOperationResponse()
            {
                Result = result.Succeeded,
                Errors = result.Errors.Select((error) => error.Description).ToList()
            };
        }

        public async Task<AuthResponse?> LoginWithTokenAsync(AuthRequest authRequest)
        {
            var user = await _userManager.FindByEmailAsync(authRequest.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, authRequest.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, authRequest.Password, false, false);
                    if (result.Succeeded)
                    {
                        var accessToken = CreateToken(user);
                        return new AuthResponse
                        {
                            Username = user.UserName,
                            UserId = user.Id,
                            Email = user.Email,
                            Token = accessToken,
                        };
                    }
                    else
                    {
                        _logger.Info("Login Failed");
                    }
                }
                else
                {
                    _logger.Info("Password is incorrect");
                }
            }
            else
            {
                _logger.Info("Can not find user with this mail");
            }
            return null;
        }

        public async Task<AuthResponse?> RegisterWithTokenAsync(AddUserRequest userRequest)
        {
            var user = new User
            {
                UserName = userRequest.Username,
                Email = userRequest.Email,
                GamesList = userRequest.GamesList,
                FriendsList = userRequest.FriendsList,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userRequest.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User could not be created");
            }
            return new AuthResponse
            {
                Username = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                Token = "",
            };
        }
    }
}
