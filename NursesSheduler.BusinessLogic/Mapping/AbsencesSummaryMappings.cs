using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary;
using NursesScheduler.BusinessLogic.Mapping.CustomResolvers;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class AbsencesSummaryMappings : Profile
    {
        public AbsencesSummaryMappings()
        {
            CreateMap<AbsencesSummary, GetAbsencesSummaryResponse>();
            CreateMap<AbsencesSummary, GetAbsencesSummaryByDepartamentResponse.AbsencesSummaryResponse>();
            CreateMap<EditAbsencesSummaryRequest, AbsencesSummary>()
                                                .ForMember(dest => dest.PTOTime, opt => opt.MapFrom<PTOTimeResolver>());
        }
    }
}
