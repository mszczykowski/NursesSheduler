using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    public class AddNurseRequest : IRequest<AddNurseResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DepartamentId { get; set; }

    }
}
