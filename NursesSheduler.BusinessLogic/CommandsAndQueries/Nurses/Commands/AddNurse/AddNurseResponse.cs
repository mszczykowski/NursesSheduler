using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    public class AddNurseResponse
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
