using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesSheduler.Application.Nurses.Queries.GetNurse
{
    internal class GetNurseResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DepartamentResponse Departament { get; set; }

        public class DepartamentResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
