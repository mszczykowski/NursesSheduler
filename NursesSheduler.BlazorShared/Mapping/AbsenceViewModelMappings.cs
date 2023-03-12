using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal class AbsenceViewModelMappings : Profile
    {
        public AbsenceViewModelMappings()
        {
            CreateMap<AbsenceViewModel, AddAbsenceRequest>();
            CreateMap<AbsenceViewModel, EditAbsenceRequest>();
        }
    }
}
