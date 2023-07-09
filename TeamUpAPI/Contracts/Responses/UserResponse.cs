using TeamUpAPI.Models;

namespace TeamUpAPI.Contracts.Responses
{
    public class UserResponse
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public int Age { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public List<Game>? GamesList { get; set; }
        public List<FriendResponse>? FriendsList { get; set; }
    }
}
