using NursesScheduler.Domain.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.Domain.Models.Settings
{
    public class UserSettings
    {
        public Departament DefaultDepartament { get; set; }
    }
}
