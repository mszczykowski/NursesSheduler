using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Commands.AddYearlyAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Queries.GetYearlyAbsencesSummary;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class YearlyAbsencesSummaryMappings : Profile
    {
        public YearlyAbsencesSummaryMappings()
        {
            CreateMap<YearlyAbsencesSummary, GetYearlyAbsencesSummaryResponse>();
            CreateMap<YearlyAbsencesSummary, AddYearlyAbsencesSummaryResponse>();
        }
    }
}
