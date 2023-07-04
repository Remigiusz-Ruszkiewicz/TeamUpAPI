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

namespace workoutapp.Controllers
{

    public class UserController : Controller
    {
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        public IUserService UserService { get; }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.User.AddUser)]
        public IActionResult AddUser([FromBody] AddUserRequest addUserRequest)
        {
            var exerciseresult = UserService.AddUser(addUserRequest);
            return Ok(exerciseresult);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.User.GetAllUsers)]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await UserService.GetUsersAsync();
            return Ok(exercises);
        }

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

        [AllowAnonymous]
        [HttpPost(ApiRoutes.User.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            return Ok(await UserService.EditUser(user));
        }

        [AllowAnonymous]
        [HttpDelete(ApiRoutes.User.DeleteUser)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            return Ok(UserService.DeleteUser(id));
        }
    }
}