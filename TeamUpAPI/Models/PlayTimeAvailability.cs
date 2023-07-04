using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TeamUpAPI.Models
{
    public class PlayTimeAvailability
    {
        [Key]
        public Guid UserId { get; set; }

    }
}
