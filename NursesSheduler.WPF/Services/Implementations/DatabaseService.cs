using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Persistance.Context;
using NursesScheduler.WPF.Services.Interfaces;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace NursesScheduler.WPF.Services.Implementations
{
    internal class DatabaseService : IDatabaseService
    {
        private readonly string dbLocation;
        public DatabaseService()
        {
            var configuration = ConfigurationManager.AppSettings;
            dbLocation = configuration["dbFileLocation"];
        }

        public Task ChangeDbPassword(string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task CreateDb(string password)
        {
            var connectionString = new SqliteConnectionStringBuilder()
            {
                DataSource = dbLocation,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = password
            }.ToString();

            var optionBuilder = new DbContextOptionsBuilder();
            optionBuilder.UseSqlite(connectionString);

            using (var context = new ApplicationDbContext(optionBuilder.Options))
            {
                await context.Database.MigrateAsync();
            }
        }

        public Task DeleteDb()
        {
            throw new System.NotImplementedException();
        }

        public string GetConnectionString(string password)
        {

            return new SqliteConnectionStringBuilder()
            {
                DataSource = dbLocation,
                Mode = SqliteOpenMode.ReadWrite,
                Password = password
            }.ToString();
        }

        public bool IsDbCreated()
        {
            return File.Exists(dbLocation);
        }
    }
}
