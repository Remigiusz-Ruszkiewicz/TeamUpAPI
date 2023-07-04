using static TeamUpAPI.Helpers.Enums;

namespace TeamUpAPI.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GameCategories Category { get; set; }
    }
}
