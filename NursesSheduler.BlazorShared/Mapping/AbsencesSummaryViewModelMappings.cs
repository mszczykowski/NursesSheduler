using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary;

namespace NursesScheduler.BlazorShared.Mapping
{
    public sealed class AbsencesSummaryViewModelMappings : Profile
    {
        public AbsencesSummaryViewModelMappings()
        {
            CreateMap<GetYearlyAbsencesSummaryResponse, AbsencesSummaryViewModel>();
            CreateMap<GetYearlyAbsencesSummaryResponse.AbsenceResponse, AbsenceViewModel>();
        }
    }
}
