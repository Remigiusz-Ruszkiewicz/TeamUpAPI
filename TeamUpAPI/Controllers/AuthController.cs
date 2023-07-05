using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Contracts;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Data;
using TeamUpAPI.Services;

namespace TeamUpAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;

        public AuthController(ITokenService tokenService, DataContext dbcontext)
        {
            TokenService = tokenService;
            _context = dbcontext;
        }

        public ITokenService TokenService { get; }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (userInDb is null)
                return Unauthorized();
            var accessToken = TokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();
            return Ok(new AuthResponse
            {
                Username = userInDb.Username,
                Email = userInDb.Email,
                Token = accessToken,
            });
        }
    }
}