using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class AbsencesSummaryViewModelMappings : Profile
    {
        public AbsencesSummaryViewModelMappings()
        {
            CreateMap<GetAbsencesSummaryResponse, AbsencesSummaryViewModel>();

            CreateMap<GetAbsencesSummaryByDepartamentResponse, AbsencesSummaryViewModel>();

            CreateMap<AbsencesSummaryViewModel, EditAbsencesSummaryRequest>();
            CreateMap<EditAbsencesSummaryResponse, AbsencesSummaryViewModel>();

            CreateMap<RecalculateAbsencesSummaryResponse, AbsencesSummaryViewModel>();
        }
    }
}
