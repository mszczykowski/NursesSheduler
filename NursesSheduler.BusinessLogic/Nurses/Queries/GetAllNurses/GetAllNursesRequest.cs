using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesRequest : IRequest<List<GetAllNursesResponse>>
    {

    }
}
