using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;
using static TeamUpAPI.Helpers.Enums;

namespace TeamUpAPI.Services
{
    public interface IGameService
    {
        public Task<ICollection<Game>> GetGamesAsync();
        public Task<Game?> GetGameByIdAsync(Guid id);
        public Task<ICollection<Game>> GetGamesByCategoryAsync(GameCategories category);
        public Task<ICollection<Game>> GetCurrentUserGamesListAsync(Guid id);
        public ICollection<GameCategories> GetGameCategories();
        public Task<Enums.OperationResult> AddToUserGamesAsync(Guid userId, List<string> gamesIds);
        public Task<Enums.OperationResult> DeleteFromUserGamesAsync(Guid userId, List<string> gamesIds);
    }
}
