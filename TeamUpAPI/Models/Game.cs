using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static TeamUpAPI.Helpers.Enums;
// ReSharper disable All

namespace TeamUpAPI.Models
{
    /// <summary>
    /// Game model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Game
    {
        /// <summary>
        /// Game id as Guid string.
        /// </summary>
        /// <example>5fd51a3c-3fac-4a17-a282-f0defb3c366e</example>
        public required string Id { get; set; }
        /// <summary>
        /// Name of the game.
        /// </summary>
        /// <example>Example Game</example>
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public required string Name { get; set; }
        /// <summary>
        /// Game category.
        /// </summary>
        /// <example>Sandbox</example>
        public GameCategories Category { get; set; }
    }
}
