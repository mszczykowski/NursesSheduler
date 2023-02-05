using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class DepartamentMappings : Profile
    {
        public DepartamentMappings()
        {
            CreateMap<Departament, GetAllDepartamentsResponse>()
                .ReverseMap();
            CreateMap<Departament, GetDepartamentResponse>()
                .ReverseMap();
            CreateMap<Departament, CreateDepartamentRequest>()
                .ReverseMap();
            CreateMap<Departament, CreateDepartamentResponse>()
                .ReverseMap();
            CreateMap<Departament, EditDepartamentRequest>()
                .ReverseMap();
            CreateMap<Departament, EditDepartamentResponse>()
                .ReverseMap();
        }
    }
}
