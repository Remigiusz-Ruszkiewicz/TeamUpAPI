namespace TeamUpAPI.Contracts.Responses
{
    /// <summary>
    /// Resonse for old auth method
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// The user's usernname.
        /// </summary>
        /// <example>bot123</example>
        public string Username { get; set; } = null!;
        /// <summary>
        /// The user's id.
        /// </summary>
        /// <example>c287ead2-bd71-4ff1-80d4-0679f196718a</example>
        public string UserId { get; set; } = null!;
        /// <summary>
        /// The user's email.
        /// </summary>
        /// <example>abc@abc@com</example>
        public string Email { get; set; } = null!;
        /// <summary>
        /// The user's token.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</example>
        public string Token { get; set; } = null!;
    }
}