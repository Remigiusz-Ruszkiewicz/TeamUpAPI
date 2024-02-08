using TeamUpAPI.Models;

namespace TeamUpAPI.Contracts.Responses
{
    /// <summary>
    /// Response for Friend
    /// </summary>
    public class FriendResponse
    {
        /// <summary>
        ///  The friend's id.
        /// </summary>
        /// <example>c287ead2-bd71-4ff1-80d4-0679f196718a</example>
        public required string Id { get; set; }
        /// <summary>
        ///  The friend's email.
        /// </summary>
        /// <example>abc@abc.com</example>
        public required string Email { get; set; }
        /// <summary>
        ///  The friend's Username.
        /// </summary>
        /// <example>bot1234</example>
        public required string Username { get; set; }
        /// <summary>
        /// The friend's age.
        /// </summary>
        /// <example>21</example>
        public int Age { get; set; }
        /// <summary>
        ///  The user's Start Hour of playing availability.
        /// </summary>
        /// <example>16</example>
        public int StartHour { get; set; }
        /// <summary>
        ///  The user's End Hour of playing availability.
        /// </summary>
        /// <example>20</example>
        public int EndHour { get; set; }
        /// <summary>
        ///  List of friend's games
        /// </summary>
        /// <example>[]</example>
        public ICollection<Game> GamesList { get; set; } = new List<Game>();
    }
}