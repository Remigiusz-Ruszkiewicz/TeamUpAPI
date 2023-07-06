using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Data;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;

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
            //await Dbcontext.Games.AddAsync(new Game() { Id = Guid.NewGuid(), Category = Enums.GameCategories.Sandbox, Name = "Minecraft" });
            //await Dbcontext.SaveChangesAsync();
            return await Dbcontext.Games.ToListAsync();
        }
    }
}
