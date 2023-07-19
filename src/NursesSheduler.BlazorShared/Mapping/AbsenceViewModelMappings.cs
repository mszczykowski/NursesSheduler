using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries.GetAbsences;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class AbsenceViewModelMappings : Profile
    {
        public AbsenceViewModelMappings()
        {
            CreateMap<AbsenceFormViewModel, AddAbsenceRequest>();
            CreateMap<AddAbsenceResponse, AbsenceViewModel>();
            CreateMap<GetAbsencesSummaryResponse.AbsenceResponse, AbsenceViewModel>();
            CreateMap<GetAbsencesResponse, AbsenceViewModel>();
        }
    }
}
