using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesSheduler.Domain.Common
{
    internal interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
