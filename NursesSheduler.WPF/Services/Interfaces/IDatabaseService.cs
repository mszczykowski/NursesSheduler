using Microsoft.EntityFrameworkCore;
using System.Security;
using System.Threading.Tasks;

namespace NursesScheduler.WPF.Services.Interfaces
{
    internal interface IDatabaseService
    {
        bool IsDbCreated();
        Task CreateDb(string password);
        void DeleteDb();
        Task<string?> TryGetConnectionStingFromPassword(string password);
        Task ChangeDbPassword(string oldPassword, string newPassword);
    }
}
