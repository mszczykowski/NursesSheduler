using AutoMapper;
using NursesScheduler.BusinessLogic.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.Departaments.Queries.GetAllDepartaments;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class DepartamentMappings : Profile
    {
        public DepartamentMappings()
        {
            CreateMap<Departament, GetAllDepartamentsResponse>()
                .ReverseMap();
            CreateMap<Departament, CreateDepartamentRequest>()
                .ReverseMap();
            CreateMap<Departament, CreateDepartamentResponse>()
                .ReverseMap();
        }
    }
}
