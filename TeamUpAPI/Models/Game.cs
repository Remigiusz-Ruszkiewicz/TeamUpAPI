using static TeamUpAPI.Helpers.Enums;

namespace TeamUpAPI.Models
{
    public class Game
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public GameCategories Category { get; set; }
    }
}
