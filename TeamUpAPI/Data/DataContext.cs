using Microsoft.EntityFrameworkCore;
using TeamUpAPI.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace TeamUpAPI.Data
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(_configuration.GetConnectionString("TeamUpDb"));
        }

        //string defaultConnectionString = "Host=customer_db;Username=postgres;Password=zaq1@WSX;Database=postgres";

        //public async Task Init()
        //{
        //    using (var connection = new NpgsqlConnection(defaultConnectionString))
        //    {
        //        connection.Open();

        //        const string checkExistenceQuery = $"SELECT 1 FROM pg_database WHERE datname='TeamUpDb'";
        //        using (var command = new NpgsqlCommand(checkExistenceQuery, connection))
        //        {
        //            var result = command.ExecuteScalar();
        //            if (result != null && result != DBNull.Value)
        //            {
        //                Console.WriteLine("Database already exists.");
        //                return;
        //            }
        //        }

        //        const string createDatabaseQuery = $"CREATE DATABASE \"TeamUpDb\"";
        //        using (var command = new NpgsqlCommand(createDatabaseQuery, connection))
        //        {
        //            command.ExecuteNonQuery();
        //            Console.WriteLine("Database created successfully.");
        //        }
        //        MigrateDatabase();
        //    }
        //}
        //internal static Action<DatabaseFacade> MigrateDatabase() =>
        //   database =>
        //   {
        //       try
        //       {
        //           database.Migrate();
        //       }
        //       catch (System.Exception ex)
        //       {
        //           string message = "Migrate database Exception: {}; connectionString = {}";
        //           if (ex.InnerException is System.Net.Sockets.SocketException)
        //           {
        //               message = "TenantContext.MigrateDatabase(): Connection to DB: {}; connectionString = {}";
        //               Console.WriteLine(message);
        //           }

        //       }
        //   };
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
