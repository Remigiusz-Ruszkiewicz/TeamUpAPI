using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamUpAPI.Contracts;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;
using TeamUpAPI.Services;

namespace TeamUpAPI.Controllers
{
    /// <summary>
    /// Game Controller
    /// </summary>
    [ApiController, Authorize]
    public class GameController : Controller
    {
        /// <summary>
        /// Base Constructor for Game Controller
        /// </summary>
        /// <param name="gameService"></param>
        public GameController(IGameService gameService)
        {
            GameService = gameService;
        }

        private IGameService GameService { get; }
        /// <summary>
        /// Get Game By Id
        /// </summary>
        [HttpGet(ApiRoutes.Game.GetGameById)]
        public async Task<IActionResult> GetGameByIdAsync([FromRoute] Guid id)
        {
            return Ok(await GameService.GetGameByIdAsync(id));
        }
        /// <summary>
        /// Get All Games List
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Game.GetAllGames)]
        public async Task<IActionResult> GetAllGames()
        {
            return Ok(await GameService.GetGamesAsync());
        }
        /// <summary>
        /// Get All Categories List
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Game.GetAllCategories)]
        public IActionResult GetAllCategories()
        {
            return Ok(GameService.GetGameCategories());
        }
        /// <summary>
        /// Get Games List By Category
        /// </summary>
        [HttpGet(ApiRoutes.Game.GetGamesByCategory)]
        public async Task<ICollection<Game>> GetGamesByCategoryAsync([FromRoute] Enums.GameCategories category)
        {
            return (await GameService.GetGamesByCategoryAsync(category)).ToList();
        }
        /// <summary>
        /// Get Current User Games List
        /// </summary>
        [HttpGet(ApiRoutes.Game.GetCurrentUserGamesList)]
        public async Task<ICollection<Game>> GetCurrentUserGamesList()
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                return (await GameService.GetCurrentUserGamesListAsync(Guid.Parse(userId))).ToList();
            }
            return new List<Game>();
        }
        /// <summary>
        /// Add to User Games List
        /// </summary>
        [HttpPost(ApiRoutes.Game.AddToUserGames)]
        public async Task<Enums.OperationResult> AddToUserGames([FromBody] List<string> gamesIds)
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                await GameService.AddToUserGamesAsync(Guid.Parse(userId!), gamesIds);
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.BadRequest;
        }
        /// <summary>
        /// Delete from User Games List
        /// </summary>
        [HttpDelete(ApiRoutes.Game.DeleteFromUserGames)]
        public async Task<Enums.OperationResult> DeleteFromUserGames([FromBody] List<string> gamesIds)
        {
            string? userId = TokenHelper.GetUserIdFromToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            if (userId != null)
            {
                await GameService.DeleteFromUserGamesAsync(Guid.Parse(userId!), gamesIds);
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.BadRequest;
        }
    }
}