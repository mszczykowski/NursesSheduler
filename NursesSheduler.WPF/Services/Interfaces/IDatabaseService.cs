using Microsoft.EntityFrameworkCore;
using System.Security;
using System.Threading.Tasks;

namespace NursesScheduler.WPF.Services.Interfaces
{
    internal interface IDatabaseService
    {
        bool IsDbCreated();

        Task CreateDb(string password);

        Task DeleteDb();

        Task ChangeDbPassword(string oldPassword, string newPassword);

        string GetConnectionString(string password);
    }
}
