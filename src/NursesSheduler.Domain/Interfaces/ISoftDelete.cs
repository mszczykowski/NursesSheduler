using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.Domain.Interfaces
{
    internal interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
