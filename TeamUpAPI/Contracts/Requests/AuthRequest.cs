namespace TeamUpAPI.Contracts.Requests
{
    public class AuthRequest
    {
        public required string Email { get; set; } = null!;
        public required string Password { get; set; } = null!;
    }
}
