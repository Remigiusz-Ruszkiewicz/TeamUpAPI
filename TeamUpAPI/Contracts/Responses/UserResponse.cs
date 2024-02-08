using TeamUpAPI.Models;

namespace TeamUpAPI.Contracts.Responses
{
    /// <summary>
    /// Response for User
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        ///  The user's id.
        /// </summary>
        /// <example>c287ead2-bd71-4ff1-80d4-0679f196718a</example>
        public required string Id { get; set; }
        /// <summary>
        ///  The user's email.
        /// </summary>
        /// <example>abc@abc.com</example>
        public required string Email { get; set; }
        /// <summary>
        ///  The user's Username.
        /// </summary>
        /// <example>bot123</example>
        public required string Username { get; set; }
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