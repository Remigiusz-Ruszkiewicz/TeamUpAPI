using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Data;
using TeamUpAPI.Helpers;
using TeamUpAPI.Helpers.Mappers;
using TeamUpAPI.Models;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace TeamUpAPI.Services
{
    public class GameService : IGameService
    {
        public GameService(DataContext dbcontext)
        {
            Dbcontext = dbcontext;
        }
        public DataContext Dbcontext { get; }
        public Task<Game?> GetGameByIdAsync(Guid id)
        {
            return Dbcontext.Games.SingleOrDefaultAsync((x) => x.Id == id.ToString());
        }
        public async Task<ICollection<Game>> GetGamesByCategoryAsync(Enums.GameCategories category)
        {
            return await Dbcontext.Games.Where((x) => x.Category == category).ToListAsync();
        }

        public ICollection<Enums.GameCategories> GetGameCategories()
        {
            return Enum.GetValues(typeof(Enums.GameCategories)).Cast<Enums.GameCategories>().ToList();
        }

        public async Task<ICollection<Game>> GetGamesAsync()
        {
            return await Dbcontext.Games.ToListAsync();
        }

        public async Task<ICollection<Game>> GetCurrentUserGamesListAsync(Guid id)
        {
            User? user = await Dbcontext.Users.SingleOrDefaultAsync((x) => x.Id == id.ToString());
            List<Game>? games = new();
            if (user != null)
            {

                if (user.GamesList != null)
                {
                    string[] gamesIds = user.GamesList.Split(';');
                    foreach (string gameId in gamesIds)
                    {
                        if (Guid.TryParse(gameId, out Guid guid))
                        {
                            Game? game = await GetGameByIdAsync(Guid.Parse(gameId));
                            if (game != null)
                            {
                                games.Add(game);
                            }
                        }

                    }
                }
            }
            return games;
        }
        public async Task<Enums.OperationResult> AddToUserGamesAsync(Guid userId, List<string> gamesIds)
        {
            User? user = await Dbcontext.Users.FirstOrDefaultAsync((x) => x.Id == userId.ToString());
            if (user != null)
            {
                foreach (string gameId in gamesIds)
                {
                    if (user.GamesList != null)
                    {
                        if (!user.GamesList.Contains(gameId))
                        {
                            user.GamesList += $"{gameId};";
                        }
                    }
                    else
                    {
                        user.GamesList = $"{gameId};";
                    }
                }
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.Error;
        }

        public async Task<Enums.OperationResult> DeleteFromUserGamesAsync(Guid userId, List<string> gamesIds)
        {
            User? user = await Dbcontext.Users.FirstOrDefaultAsync((x) => x.Id == userId.ToString());
            if (user != null)
            {
                if (user.GamesList != null)
                {
                    foreach (string gameId in gamesIds)
                    {
                        if (user.GamesList.Contains(gameId))
                        {
                            user.GamesList = user.GamesList.Replace($"{gameId};", "");
                        }
                    }
                }
                await Dbcontext.SaveChangesAsync();
                return Enums.OperationResult.Ok;
            }
            return Enums.OperationResult.Error;
        }
    }
}
