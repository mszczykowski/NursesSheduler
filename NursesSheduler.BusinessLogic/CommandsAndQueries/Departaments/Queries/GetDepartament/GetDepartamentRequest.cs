﻿
using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament
{
    public class GetDepartamentRequest : IRequest<GetDepartamentResponse>
    {
        public int Id { get; set; }
    }
}
