using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.AddYearlyAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Commands.InitializeYearlyAbsencesSummary;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class AbsencesSummaryMappings : Profile
    {
        public AbsencesSummaryMappings()
        {
            CreateMap<AbsencesSummary, GetYearlyAbsencesSummaryResponse>();

            CreateMap<AbsencesSummary, AddYearlyAbsencesSummaryResponse>();

            CreateMap<AbsencesSummary, InitializeYearlyAbsencesSummariesResponse.YearlyAbsencesSummaryResponse>
        }
    }
}
