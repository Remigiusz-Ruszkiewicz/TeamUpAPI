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
    /// <summary>
    /// Authorization Controller
    /// </summary>
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Base Constructor For Authorization Controller
        /// </summary>
        /// <param name="tokenService"></param>
        public AuthController(ITokenService tokenService)
        {
            TokenService = tokenService;
        }

        private ITokenService TokenService { get; }
        /// <summary>
        /// Login and get token
        /// </summary>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            AuthResponse? authResponse = await TokenService.LoginAsync(request);
            if (authResponse != null)
            {
                return Ok(authResponse);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}