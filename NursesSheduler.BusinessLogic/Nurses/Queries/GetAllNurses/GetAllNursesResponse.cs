using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesResponse
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
