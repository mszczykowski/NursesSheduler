using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class AbsenceViewModelMappings : Profile
    {
        public AbsenceViewModelMappings()
        {
            CreateMap<AbsenceViewModel, AddAbsenceRequest>();
            CreateMap<AbsenceViewModel, EditAbsenceRequest>();
            CreateMap<AddAbsenceResponse, AbsenceViewModel>();
            CreateMap<GetAbsencesSummaryResponse.AbsenceResponse, AbsenceViewModel>();
        }
    }
}
