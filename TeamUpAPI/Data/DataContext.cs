using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TeamUpAPI.Models;

namespace TeamUpAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
