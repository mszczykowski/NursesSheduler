using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Queries.GetYearlyAbsencesSummary;

namespace NursesScheduler.BlazorShared.Mapping
{
    public sealed class YearlyAbsencesSummaryViewModelMappings : Profile
    {
        public YearlyAbsencesSummaryViewModelMappings()
        {
            CreateMap<YearlyAbsencesSummaryViewModel, GetYearlyAbsencesSummaryResponse>();
        }
    }
}
