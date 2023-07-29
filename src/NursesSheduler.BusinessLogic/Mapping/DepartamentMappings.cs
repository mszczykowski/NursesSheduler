using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.PickDepartament;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class DepartamentMappings : Profile
    {
        public DepartamentMappings()
        {
            CreateMap<Departament, GetAllDepartamentsResponse>();
            CreateMap<Departament, GetDepartamentResponse>()
                .ForMember(dest => dest.FirstQuarterStart,
                            opt => opt.MapFrom(src => src.DepartamentSettings.FirstQuarterStart))
                .ForMember(dest => dest.DefaultGeneratorRetryValue,
                                opt => opt.MapFrom(src => src.DepartamentSettings.DefaultGeneratorRetryValue));
            CreateMap<CreateDepartamentRequest, Departament>();
            CreateMap<Departament, CreateDepartamentResponse>();
            CreateMap<EditDepartamentRequest, Departament>();
            CreateMap<Departament, EditDepartamentResponse>();
            CreateMap<Departament, PickDepartamentResponse>()
                .ForMember(dest => dest.UseTeamsSetting,
                            opt => opt.MapFrom(src => src.DepartamentSettings.UseTeams));
        }
    }
}
