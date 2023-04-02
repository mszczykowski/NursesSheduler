using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class AbsencesSummaryViewModelMappings : Profile
    {
        public AbsencesSummaryViewModelMappings()
        {
            CreateMap<GetAbsencesSummaryResponse, AbsencesSummaryViewModel>();
            CreateMap<GetAbsencesSummaryByDepartamentResponse.AbsencesSummaryResponse, AbsencesSummaryViewModel>();
            CreateMap<AbsencesSummaryViewModel, AbsencesSummaryEditViewModel>();
            CreateMap<RecalculateAbsencesSummaryResponse, AbsencesSummaryEditViewModel>();
            CreateMap<AbsencesSummaryEditViewModel, EditAbsencesSummaryRequest>();
        }
    }
}
