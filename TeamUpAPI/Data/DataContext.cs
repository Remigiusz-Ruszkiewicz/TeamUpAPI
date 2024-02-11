using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TeamUpAPI.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("TeamUpDb"));
        }
        public DbSet<Game> Games { get; set; }
    }
}
