using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamUpAPI.Contracts;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Models;
using TeamUpAPI.Services;

namespace TeamUpAPI.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
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
        [AllowAnonymous]
        [HttpPost(ApiRoutes.User.AddUser)]
        public IActionResult AddUser([FromBody] AddUserRequest addUserRequest)
        {
            var exerciseresult = UserService.AddUser(addUserRequest);
            return Ok(exerciseresult);
        }
        /// <summary>
        /// Get All Users List
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.User.GetAllUsers)]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await UserService.GetUsersAsync();
            return Ok(exercises);
        }
        /// <summary>
        /// Get User By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.User.GetUserById)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var exercise = await UserService.GetUserByIdAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }
        /// <summary>
        /// Update User
        /// </summary>
        [AllowAnonymous]
        [HttpPut(ApiRoutes.User.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            return Ok(await UserService.EditUser(user));
        }
        /// <summary>
        /// Delete User Based On Id
        /// </summary>
        [AllowAnonymous]
        [HttpDelete(ApiRoutes.User.DeleteUser)]
        public IActionResult DeleteUser([FromRoute] Guid id)
        {
            return Ok(UserService.DeleteUser(id));
        }
        /// <summary>
        /// Get List Of User Friends Based On User Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.User.GetUserFriends)]
        public async Task<IActionResult> GetUserFriends([FromRoute] Guid id)
        {
            return Ok(await UserService.GetUserFriendsAsync(id));
        }
    }
}