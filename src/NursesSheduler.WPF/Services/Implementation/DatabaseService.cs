using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Infrastructure.Context;
using NursesScheduler.WPF.Models.Exceptions;
using NursesScheduler.WPF.Services.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace NursesScheduler.WPF.Services.Implementation
{
    internal sealed class DatabaseService : IDatabaseService
    {
        private readonly string dbLocation;
        public DatabaseService()
        {
            dbLocation = ConfigurationManager.AppSettings["dbFileLocation"];
        }

        public async Task ChangeDbPassword(string oldPassword, string newPassword)
        {
            var connectionString = await TryGetConnectionStingFromPassword(oldPassword);

            if (connectionString == null)
                throw new InvalidPasswordException();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT quote($newPassword);";
                command.Parameters.AddWithValue("$newPassword", newPassword);
                var quotedNewPassword = (string)command.ExecuteScalar();

                command.CommandText = "PRAGMA rekey = " + quotedNewPassword;
                command.Parameters.Clear();

                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        private string BuildConnectionString(string password, SqliteOpenMode mode)
        {
            return new SqliteConnectionStringBuilder
            {
                DataSource = dbLocation,
                Mode = mode,
                Password = password
            }.ToString();
        }

        public async Task CreateDb(string password)
        {
            var optionBuilder = new DbContextOptionsBuilder();
            optionBuilder.UseSqlite(BuildConnectionString(password, SqliteOpenMode.ReadWriteCreate));
            
            using (var context = new ApplicationDbContext(optionBuilder.Options))
            {
                await context.Database.MigrateAsync();
                await context.Database.CloseConnectionAsync();
                await context.DisposeAsync();
            }
        }

        public void DeleteDb()
        {
            if (IsDbCreated())
            {
                File.Delete(dbLocation);
            }
        }

        public bool IsDbCreated()
        {
            return File.Exists(dbLocation);
        }

        public async Task<string?> TryGetConnectionStingFromPassword(string password)
        {
            if (!IsDbCreated()) 
                return null;

            var optionsBuilder = new DbContextOptionsBuilder();
            var connectionString = BuildConnectionString(password, SqliteOpenMode.ReadWrite);
            optionsBuilder.UseSqlite(connectionString);

            using (var context = new DbContext(optionsBuilder.Options))
            {
                if(await context.Database.CanConnectAsync())
                    return connectionString;

                return null;
            }
        }
    }
}
