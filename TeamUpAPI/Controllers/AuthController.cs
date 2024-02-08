using Asp.Versioning;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Contracts;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Data;
using TeamUpAPI.Models;
using TeamUpAPI.Services;

namespace TeamUpAPI.Controllers
{
    /// <summary>
    /// Manages user authentication processes such as login and registration within the TeamUp application.
    /// Supports operations for account creation, authentication, and secure token generation.
    /// </summary>
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Constructs an AuthController with a dependency on the IAuthService.
        /// This service is responsible for executing authentication-related operations, including user validation and token management.
        /// </summary>
        /// <param name="authService">The service that implements authentication logic.</param>
        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        private IAuthService AuthService { get; }
        /// <summary>
        /// Authenticates the user with provided credentials and returns an operation response indicating the success or failure of the login attempt.
        /// </summary>
        /// <param name="authRequest">The user's login credentials, including identifiers like username or email, and password.</param>
        /// <returns>An ActionResult containing the authentication operation response, which includes status and any relevant user or error information.</returns>
        [ApiVersion("1.1")]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<ActionResult<AuthOperationResponse>> Login([FromBody] AuthRequest authRequest)
        {
            AuthOperationResponse authResponse = await AuthService.LoginAsync(authRequest);
            if (authResponse.Result)
            {
                return Ok(authResponse);
            }
            else
            {
                return BadRequest(authResponse);
            }
        }
        /// <summary>
        /// Registers a new user with the provided account details and returns an operation response indicating the success or failure of the registration attempt.
        /// </summary>
        /// <param name="userRequest">The details for the new user account, such as username, password, and email.</param>
        /// <returns>An ActionResult containing the authentication operation response, detailing the result of the registration process.</returns>
        [ApiVersion("1.1")]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Register)]
        public async Task<ActionResult<AuthOperationResponse>> Register([FromBody] AddUserRequest userRequest)
        {
            AuthOperationResponse authResponse = await AuthService.RegisterAsync(userRequest);
            if (authResponse.Result)
            {
                return Ok(authResponse);
            }
            else
            {
                return BadRequest(authResponse);
            }
        }
        /// <summary>
        /// (Deprecated) Logs in a user and sets authentication cookies without returning an explicit token in the response body.
        /// This approach is maintained for backward compatibility and should be used with consideration of newer authentication methods.
        /// </summary>
        /// <param name="authRequest">The user's login credentials.</param>
        /// <returns>An Ok response with authentication cookies set if login is successful; otherwise, an Unauthorized response.</returns>
        [ApiVersion("1.1")]
        [ApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.LoginWithToken)]
        public async Task<ActionResult<AuthResponse>> LoginWithToken([FromBody] AuthRequest authRequest)
        {
            AuthResponse? authResponse = await AuthService.LoginWithTokenAsync(authRequest);
            if (authResponse != null)
            {
                Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                Response.Cookies.Append("X-Username", authResponse.Username, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                Response.Cookies.Append("X-Id", authResponse.UserId, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                return Ok(authResponse);
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// (Deprecated) Registers a new user and sets authentication cookies, similarly to LoginWithToken, without an explicit token in the response body.
        /// Maintains backward compatibility while encouraging the use of more secure and explicit token-based methods.
        /// </summary>
        /// <param name="userRequest">Details for the user account to be registered.</param>
        /// <returns>An Ok response with authentication cookies set if registration is successful; otherwise, an Unauthorized response.</returns>
        [ApiVersion("1.1")]
        [ApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.RegisterWithToken)]
        public async Task<ActionResult<AuthResponse>> RegisterWithToken([FromBody] AddUserRequest userRequest)
        {
            AuthResponse? authResponse = await AuthService.RegisterWithTokenAsync(userRequest);
            if (authResponse != null)
            {
                Response.Cookies.Append("X-Access-Token", authResponse.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                Response.Cookies.Append("X-Username", authResponse.Username, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                Response.Cookies.Append("X-Id", authResponse.UserId, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                return Ok(authResponse);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}