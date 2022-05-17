using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesSheduler.Application.Nurses.Commands.CreateNurse
{
    public class CreateNurseRequest : IRequest<CreateNurseResponse>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int DepartamentId { get; set; }

    }
}
