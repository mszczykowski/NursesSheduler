using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetAllNurses;
using NursesScheduler.Domain.DatabaseModels;

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
