using System.ComponentModel.DataAnnotations;

namespace TeamUpAPI.Models
{
    public class PlayTimeAvailability
    {
        [Key]
        public required string UserId { get; set; }
    }
}
