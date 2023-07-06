namespace TeamUpAPI.Models
{
    public class User
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int Age { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public string? FriendsList { get; set; }
        public string? GamesList { get; set; }
    }
}
