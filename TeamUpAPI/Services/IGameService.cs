using TeamUpAPI.Models;
using static TeamUpAPI.Helpers.Enums;

namespace TeamUpAPI.Services
{
    public interface IGameService
    {
        public Task<ICollection<Game>> GamesAsync();
        public Task<Game?> GameByIdAsync(Guid id);
        public Task<ICollection<Game>> GamesByCategoryAsync(GameCategories category);
        public Task<ICollection<Game>> CurrentUserGamesListAsync();
        public ICollection<GameCategories> GameCategories();
        public Task<OperationResult> AddToUserGamesAsync(List<Guid> gamesIds);
        public Task<OperationResult> DeleteFromUserGamesAsync(List<Guid> gamesIds);
    }
}
