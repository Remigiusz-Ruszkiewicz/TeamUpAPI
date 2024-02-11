namespace TeamUpAPI.Contracts.Requests
{
    /// <summary>
    /// Model for login
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// The user's email.
        /// </summary>
        /// <example>abc@abc@com</example>
        public required string Email { get; set; } = null!;
        /// <summary>
        /// The user's password.
        /// </summary>
        /// <example>zaq1@WSX</example>
        public required string Password { get; set; } = null!;
    }
}
