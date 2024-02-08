using Microsoft.AspNetCore.Identity;

namespace TeamUpAPI.Models
{
    /// <summary>
    /// A user.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// The user's age.
        /// </summary>
        /// <example>20</example>
        public int Age { get; set; }
        /// <summary>
        /// The user's Start Hour of playing availability.
        /// </summary>
        /// <example>16</example>
        public int StartHour { get; set; }
        /// <summary>
        ///  The user's End Hour of playing availability.
        /// </summary>
        /// <example>20</example>
        public int EndHour { get; set; }
        /// <summary>
        /// List of User Friends.
        /// </summary>
        /// <example>[]</example>
        public required ICollection<Friend> FriendsList { get; set; } = new List<Friend>();
        /// <summary>
        /// List of User Games.
        /// </summary>
        /// <example>[]</example>
        public required ICollection<Game> GamesList { get; set; } = new List<Game>();
    }
}
