using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament;
using NursesScheduler.Domain.DatabaseModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseMappings : Profile
    {
        public NurseMappings()
        {
            CreateMap<Nurse, AddNurseRequest>()
                .ReverseMap();
            CreateMap<Nurse, AddNurseResponse>()
                .ReverseMap();
            CreateMap<Nurse, GetNursesFromDepartamentResponse>()
                .ReverseMap();
        }
    }
}
