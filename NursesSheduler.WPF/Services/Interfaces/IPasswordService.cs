using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.WPF.Services.Interfaces
{
    internal interface IPasswordService
    {
        Task<bool> IsPasswordValid(string connectionString); 
    }
}
