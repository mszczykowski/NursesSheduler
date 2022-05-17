using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesSheduler.Application.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesRequest : IRequest<List<GetAllNursesResponse>>
    {

    }
}
