using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamUpAPI.Contracts;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Contracts.Responses;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;
using TeamUpAPI.Services;

namespace TeamUpAPI.Controllers
{
    /// <summary>
    /// Manages user-related operations such as creating, retrieving, updating, and deleting user accounts.
    /// Also handles operations related to user relationships, like managing friends lists and recommending users.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController, Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Initializes a new instance of UserController with a dependency on IUserService.
        /// IUserService is responsible for executing user-related operations, including data management and relationship handling.
        /// </summary>
        /// <param name="userService">The service that implements user logic.</param>
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        private IUserService UserService { get; }
        /// <summary>
        /// Adds a new user to the application using the provided user details.
        /// </summary>
        /// <param name="addUserRequest">The details of the user to add, including username, password, and email.</param>
        /// <returns>An IActionResult indicating the success or failure of the add operation.</returns>
        [HttpPost(ApiRoutes.User.AddUser)]
        public IActionResult AddUser([FromBody] AddUserRequest addUserRequest)
        {
            return Ok(UserService.AddUser(addUserRequest));
        }
        /// <summary>
        /// Retrieves a list of all users registered in the application.
        /// </summary>
        /// <returns>An IActionResult containing a list of all users.</returns>
        [HttpGet(ApiRoutes.User.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await UserService.GetUsersAsync());
        }
        /// <summary>
        /// Retrieves detailed information about a specific user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>An IActionResult containing the user's details if found; otherwise, NotFound.</returns>
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
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="user">The updated user information.</param>
        /// <returns>An IActionResult indicating the success or failure of the update operation.</returns>
        [HttpPut(ApiRoutes.User.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            return Ok(await UserService.EditUser(user));
        }
        /// <summary>
        /// Deletes the user identified by the Id stored in the request's cookies.
        /// </summary>
        /// <returns>An IActionResult indicating the success or failure of the delete operation.</returns>
        [HttpDelete(ApiRoutes.User.DeleteUser)]
        public IActionResult DeleteUser()
        {
            string? userId = HttpContext.Request.Cookies[key: "X-Id"];
            if (userId != null)
            {
                return Ok(UserService.DeleteUser(Guid.Parse(userId!)));
            }
            return NotFound("Bad Token try with valid one");
        }
        /// <summary>
        /// Retrieves a list of friends for the currently authenticated user.
        /// </summary>
        /// <returns>An IActionResult containing a list of user friends.</returns>
        [HttpGet(ApiRoutes.User.GetUserFriends)]
        public async Task<IActionResult> GetUserFriends()
        {
            return Ok(await UserService.GetUserFriendsAsync());
        }
        /// <summary>
        /// Adds one or more users to the currently authenticated user's friends list.
        /// </summary>
        /// <param name="friendsIds">A list of unique identifiers for the users to be added as friends.</param>
        /// <returns>A Task with an OperationResult indicating the success or failure of the addition.</returns>
        [HttpPost(ApiRoutes.User.AddToUserFriends)]
        public Task<Enums.OperationResult> AddToUserFriends([FromBody] List<Guid> friendsIds)
        {
            return UserService.AddToUserFriendsAsync(friendsIds);
        }
        /// <summary>
        /// Removes a user from the currently authenticated user's friends list.
        /// </summary>
        /// <param name="id">The unique identifier of the friend to remove.</param>
        /// <returns>A Task with an OperationResult indicating the success or failure of the removal.</returns>
        [HttpDelete(ApiRoutes.User.DeleteFromUserFriends)]
        public Task<Enums.OperationResult> DeleteFromUserFriends([FromRoute] Guid id)
        {
            return UserService.DeleteFromUserFriendsAsync(id);
        }
        /// <summary>
        /// Retrieves a list of recommended users, optionally filtered by a specific game.
        /// </summary>
        /// <param name="gameId">An optional parameter to filter recommendations based on a game's unique identifier.</param>
        /// <returns>A Task containing a collection of recommended UserResponse objects.</returns>
        [HttpGet(ApiRoutes.User.GetRecomendedUsers)]
        public Task<ICollection<UserResponse>> GetRecomendedUsers(Guid? gameId = null)
        {

            if (gameId != null)
            {
                return UserService.GetRecomendedUsersByGameAsync((Guid)gameId);
            }
            else
            {
                return UserService.GetRecomendedUsersAsync();
            }
        }
        ///// <summary>
        ///// Get Recomended Users By Game
        ///// </summary>
        //[HttpGet(ApiRoutes.User.GetRecomendedUsersByGame)]
        //public async Task<ICollection<UserResponse>> GetRecomendedUsersByGame([FromRoute] Guid gameId)
        //{
        //    string? userId = HttpContext.Request.Cookies[key: "X-Id"];
        //    if (userId != null)
        //    {
        //        var result = await UserService.GetRecomendedUsersByGameAsync(Guid.Parse(userId!), gameId.ToString());
        //        return result;
        //    }
        //    return new List<UserResponse>();
        //}
        /// <summary>
        /// Retrieves information about the currently authenticated user.
        /// </summary>
        /// <returns>A Task containing the UserResponse of the current user.</returns>
        [HttpGet(ApiRoutes.User.GetCurrentUserInfo)]
        public Task<UserResponse?> GetCurrentUserInfo()
        {
            return UserService.GeturrentUserAsync();
        }
    }
}