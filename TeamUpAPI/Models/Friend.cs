using System.Diagnostics.CodeAnalysis;

namespace TeamUpAPI.Models
{
    [ExcludeFromCodeCoverage]
    public class Friend
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public int Age { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public required ICollection<Friend> FriendsList { get; set; }
        public required ICollection<Game> GamesList { get; set; }
    }
}
