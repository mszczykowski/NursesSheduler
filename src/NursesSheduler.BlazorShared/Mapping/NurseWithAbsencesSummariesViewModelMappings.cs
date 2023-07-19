using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class NurseWithAbsencesSummariesViewModelMappings : Profile
    {
        public NurseWithAbsencesSummariesViewModelMappings()
        {
            CreateMap<GetAbsencesSummaryByDepartamentResponse, NurseWithAbsencesSummariesViewModel>();
        }
    }
}
