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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Game.GetGamesByCategory)]
        public async Task<ICollection<Game>> GetGamesByCategoryAsync([FromRoute] Enums.GameCategories category)
        {
            return (await GameService.GetGamesByCategoryAsync(category)).ToList();
        }
    }
}