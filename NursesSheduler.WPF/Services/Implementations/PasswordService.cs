using Microsoft.EntityFrameworkCore;
using NursesScheduler.WPF.Services.Interfaces;
using System.Threading.Tasks;

namespace NursesScheduler.WPF.Services.Implementations
{
    internal class PasswordService : IPasswordService
    {
        public async Task<bool> IsPasswordValid(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite(connectionString);

            using (var context = new DbContext(optionsBuilder.Options))
            {
                return await context.Database.CanConnectAsync();
            }
        }
    }
}
