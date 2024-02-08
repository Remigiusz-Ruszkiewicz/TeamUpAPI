namespace TeamUpAPI.Contracts.Responses
{
    /// <summary>
    /// Model Auth Response With Result and Errors List
    /// </summary>
    public class AuthOperationResponse
    {
        /// <summary>
        /// Result of operation true when succes false when fail.
        /// </summary>
        /// <example>True</example>
        public bool Result { get; set; }
        /// <summary>
        /// Errors list from response.
        /// </summary>
        /// <example>[]</example>
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}