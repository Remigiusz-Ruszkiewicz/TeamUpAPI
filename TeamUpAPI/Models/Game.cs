using System.ComponentModel.DataAnnotations;
using static TeamUpAPI.Helpers.Enums;

namespace TeamUpAPI.Models
{
    public class Game
    {
        public required string Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public required string Name { get; set; }
        public GameCategories Category { get; set; }
    }
}
