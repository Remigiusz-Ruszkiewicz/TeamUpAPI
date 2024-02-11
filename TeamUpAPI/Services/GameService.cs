using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TeamUpAPI.Data;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public class GameService : IGameService
    {
        public GameService(DataContext dbcontext, UserManager<User> userManager)
        {
            Dbcontext = dbcontext;
            _userManager = userManager;
        }

        private DataContext Dbcontext { get; }
        private readonly UserManager<User> _userManager;
        public Task<Game?> GameByIdAsync(Guid id)
        {
            return Dbcontext.Games.SingleOrDefaultAsync((x) => x.Id == id.ToString());
        }
        public async Task<ICollection<Game>> GamesByCategoryAsync(Enums.GameCategories category)
        {
            return await Dbcontext.Games.Where((x) => x.Category == category).ToListAsync();
        }

        public ICollection<Enums.GameCategories> GameCategories()
        {
            return Enum.GetValues(typeof(Enums.GameCategories)).Cast<Enums.GameCategories>().ToList();
        }

        public async Task<ICollection<Game>> GamesAsync()
        {
            return await Dbcontext.Games.ToListAsync();
        }

        public async Task<ICollection<Game>> CurrentUserGamesListAsync()
        {
            var user = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            return user.GamesList;
        }
        public async Task<Enums.OperationResult> AddToUserGamesAsync(List<Guid> gamesIds)
        {
            var user = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            if (user != null)
            {
                foreach (Guid gameId in gamesIds)
                {
                    if (user.GamesList.All(x => x.Id != gameId.ToString()))
                    {
                        Game? game = await GameByIdAsync(gameId);
                        if (game != null)
                        {
                            user.GamesList.Add(game);
                        }
                        else
                        {
                            return Enums.OperationResult.BadRequest;
                        }
                    }
                }
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.Error;
        }

        public async Task<Enums.OperationResult> DeleteFromUserGamesAsync(List<Guid> gamesIds)
        {
            var user = await _userManager.GetUserAsync(ClaimsPrincipal.Current);
            if (user != null)
            {
                foreach (Guid gameId in gamesIds)
                {
                    Game? game = await GameByIdAsync(gameId);
                    if (game != null)
                    {
                        user.GamesList.Remove(game);
                    }
                    else
                    {
                        return Enums.OperationResult.BadRequest;
                    }
                }
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.Error;
        }
    }
}
