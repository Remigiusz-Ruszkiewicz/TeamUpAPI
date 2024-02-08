using Microsoft.AspNetCore.Identity;
using TeamUpAPI.Models;

namespace TeamUpAPI.Contracts.Requests
{
    /// <summary>
    /// Request for adding User
    /// </summary>
    public class AddUserRequest
    {
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
        ///  The user's password.
        /// </summary>
        /// <example>zaq1@WSX</example>
        public required string Password { get; set; }
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