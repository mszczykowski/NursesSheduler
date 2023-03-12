using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.EditNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament;
using NursesScheduler.Domain.DomainModels;

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
            CreateMap<Nurse, GetNurseResponse>()
                .ReverseMap();
            CreateMap<Nurse, EditNurseRequest>()
                .ReverseMap();
            CreateMap<Nurse, EditNurseResponse>()
                .ReverseMap();
        }
    }
}
