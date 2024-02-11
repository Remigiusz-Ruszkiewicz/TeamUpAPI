using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TeamUpAPI.Contracts;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;
using TeamUpAPI.Services;

namespace TeamUpAPI.Controllers
{
    /// <summary>
    /// Manages game-related operations, including retrieving game information, managing user game lists, and handling game categories.
    /// Requires authorization for most operations to ensure user data privacy and integrity.
    /// </summary>
    [EnableRateLimiting("fixed")]
    [ApiVersion("1.0")]
    [ApiController, Authorize]
    public class GameController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the GameController with a dependency on IGameService.
        /// This service provides functionality for accessing and manipulating game data and user game collections.
        /// </summary>
        /// <param name="gameService">The service responsible for game data operations.</param>
        public GameController(IGameService gameService)
        {
            GameService = gameService;
        }

        private IGameService GameService { get; }
        /// <summary>
        /// Retrieves detailed information about a game by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game to retrieve.</param>
        /// <returns>An IActionResult containing the game details if found; otherwise, NotFound.</returns>
        [HttpGet(ApiRoutes.Game.GameById)]
        public async Task<IActionResult> GameByIdAsync([FromRoute] Guid id)
        {
            return Ok(await GameService.GameByIdAsync(id));
        }
        /// <summary>
        /// Retrieves a list of all games available in the application.
        /// </summary>
        /// <returns>An IActionResult containing a list of all games.</returns>
        [HttpGet(ApiRoutes.Game.AllGames)]
        public async Task<IActionResult> AllGames()
        {
            return Ok(await GameService.GamesAsync());
        }
        /// <summary>
        /// Retrieves a list of all game categories defined within the application.
        /// </summary>
        /// <returns>An IActionResult containing a list of game categories.</returns>
        [Authorize]
        [HttpGet(ApiRoutes.Game.AllCategories)]
        public IActionResult AllCategories()
        {
            return Ok(GameService.GameCategories());
        }
        /// <summary>
        /// Retrieves a list of games filtered by a specific category.
        /// </summary>
        /// <param name="category">The category to filter games by.</param>
        /// <returns>A collection of games that belong to the specified category.</returns>
        [HttpGet(ApiRoutes.Game.GamesByCategory)]
        public async Task<ICollection<Game>> GamesByCategoryAsync([FromRoute] Enums.GameCategories category)
        {
            return (await GameService.GamesByCategoryAsync(category)).ToList();
        }
        /// <summary>
        /// Retrieves a list of games associated with the current authenticated user's account.
        /// </summary>
        /// <returns>A Task representing an asynchronous operation containing a collection of games.</returns>
        [HttpGet(ApiRoutes.Game.CurrentUserGamesList)]
        public Task<ICollection<Game>> CurrentUserGamesList()
        {
            return GameService.CurrentUserGamesListAsync();
        }
        /// <summary>
        /// Adds a list of games to the current authenticated user's game collection.
        /// </summary>
        /// <param name="gamesIds">A list of unique identifiers for the games to be added.</param>
        /// <returns>An IActionResult indicating the success or failure of the addition operation.</returns>
        [ValidateAntiForgeryToken]
        [HttpPost(ApiRoutes.Game.AddToUserGames)]
        public async Task<IActionResult> AddToUserGames([FromBody] List<Guid> gamesIds)
        {

            Enums.OperationResult result = await GameService.AddToUserGamesAsync(gamesIds);
            switch (result)
            {
                case Enums.OperationResult.Ok:
                    return Ok();
                default:
                    return BadRequest();
            }
        }
        /// <summary>
        /// Removes a list of games from the current authenticated user's game collection.
        /// </summary>
        /// <param name="gamesIds">A list of unique identifiers for the games to be removed.</param>
        /// <returns>A Task representing an asynchronous operation with an OperationResult indicating the success or failure of the removal.</returns>
        [HttpDelete(ApiRoutes.Game.DeleteFromUserGames)]
        public Task<Enums.OperationResult> DeleteFromUserGames([FromBody] List<Guid> gamesIds)
        {
            return GameService.DeleteFromUserGamesAsync(gamesIds);
        }
    }
}