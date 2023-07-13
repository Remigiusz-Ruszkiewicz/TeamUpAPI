using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TeamUpAPI.Contracts;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;
using TeamUpAPI.Services;
using TokenHelper = TeamUpAPI.Helpers.TokenHelper;

namespace TeamUpAPI.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController, Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Base Constructor for User Service
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        private IUserService UserService { get; }
        /// <summary>
        /// Add User
        /// </summary>
        [HttpPost(ApiRoutes.User.AddUser)]
        public IActionResult AddUser([FromBody] AddUserRequest addUserRequest)
        {
            return Ok(UserService.AddUser(addUserRequest));
        }
        /// <summary>
        /// Get All Users List
        /// </summary>
        [HttpGet(ApiRoutes.User.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                return Ok(UserService.GetUsersAsync(Guid.Parse(userId!)));
            }
            return NotFound("Bad Token try with valid one");
        }
        /// <summary>
        /// Get User By Id
        /// </summary>
        [HttpGet(ApiRoutes.User.GetUserById)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var user = await UserService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        /// <summary>
        /// Update User
        /// </summary>
        [HttpPut(ApiRoutes.User.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            return Ok(await UserService.EditUser(user));
        }
        /// <summary>
        /// Delete User Based On Id
        /// </summary>
        [HttpDelete(ApiRoutes.User.DeleteUser)]
        public IActionResult DeleteUser()
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                return Ok(UserService.DeleteUser(Guid.Parse(userId!)));
            }
            return NotFound("Bad Token try with valid one");
        }
        /// <summary>
        /// Get List Of User Friends Based On User Id
        /// </summary>
        [HttpGet(ApiRoutes.User.GetUserFriends)]
        public async Task<IActionResult> GetUserFriends()
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                return Ok(await UserService.GetUserFriendsAsync(Guid.Parse(userId!)));
            }
            return NotFound("Bad Token try with valid one");
        }
        /// <summary>
        /// Add to User Friends List
        /// </summary>
        [HttpPost(ApiRoutes.User.AddToUserFriends)]
        public async Task<Enums.OperationResult> AddToUserFriends([FromBody] List<string> friendsIds)
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                await UserService.AddToUserFriendsAsync(Guid.Parse(userId!), friendsIds);
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.BadRequest;
        }
        /// <summary>
        /// Delete from User Friends List
        /// </summary>
        [HttpDelete(ApiRoutes.User.DeleteFromUserFriends)]
        public async Task<Enums.OperationResult> DeleteFromUserFriends([FromBody] List<string> friendsIds)
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                await UserService.DeleteFromUserFriendsAsync(Guid.Parse(userId!), friendsIds);
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.BadRequest;
        }
        /// <summary>
        /// Get Recomended Users (optional game id parameter)
        /// </summary>
        [HttpGet(ApiRoutes.User.GetRecomendedUsers)]
        public Task<ICollection<UserResponse>> GetRecomendedUsers(Guid? gameId = null)
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                if (gameId != null)
                {
                    return UserService.GetRecomendedUsersByGameAsync(Guid.Parse(userId!), gameId.ToString());
                }
                else
                {
                    return UserService.GetRecomendedUsersAsync(Guid.Parse(userId!));
                }

            }
            return Task.FromResult<ICollection<UserResponse>>(new List<UserResponse>());
        }
        ///// <summary>
        ///// Get Recomended Users By Game
        ///// </summary>
        //[HttpGet(ApiRoutes.User.GetRecomendedUsersByGame)]
        //public async Task<ICollection<UserResponse>> GetRecomendedUsersByGame([FromRoute] Guid gameId)
        //{
        //    string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        //    if (userId != null)
        //    {
        //        var result = await UserService.GetRecomendedUsersByGameAsync(Guid.Parse(userId!), gameId.ToString());
        //        return result;
        //    }
        //    return new List<UserResponse>();
        //}
        /// <summary>
        /// Get Current User
        /// </summary>
        [HttpGet(ApiRoutes.User.GetCurrentUserInfo)]
        public async Task<UserResponse?> GetCurrentUserInfo()
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                var result = await UserService.GetUserByIdAsync(Guid.Parse(userId!));
                return result;
            }
            return null;
        }
        /// <summary>
        /// Get Current User
        /// </summary>
        //[AllowAnonymous]
        [HttpGet(ApiRoutes.User.GetCurrentUserInfo)]
        public async Task<UserResponse?> GetCurrentUserInfo()
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                var result = await UserService.GetUserByIdAsync(Guid.Parse(userId!));
                return result;
            }
            return null;
        }
    }
}