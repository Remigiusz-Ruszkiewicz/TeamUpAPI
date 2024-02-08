using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TeamUpAPI.Data
{
    public class DataContext : IdentityDbContext<User>//: DbContext
    {
        protected readonly IConfiguration _configuration;
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
