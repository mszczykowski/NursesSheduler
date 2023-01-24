using AutoMapper;
using NursesScheduler.BusinessLogic.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.Nurses.Queries.GetAllNurses;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseMappings : Profile
    {
        public NurseMappings()
        {
            CreateMap<Nurse, CreateDepartamentRequest>()
                .ReverseMap();
            CreateMap<Nurse, CreateDepartamentResponse>()
                .ReverseMap();
            CreateMap<Nurse, GetAllNursesResponse>()
                .ReverseMap();
        }
    }
}
