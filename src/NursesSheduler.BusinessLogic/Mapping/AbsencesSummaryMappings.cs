using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class AbsencesSummaryMappings : Profile
    {
        public AbsencesSummaryMappings()
        {
            CreateMap<AbsencesSummary, GetAbsencesSummaryResponse>();

            CreateMap<AbsencesSummary, GetAbsencesSummaryByDepartamentResponse>();

            CreateMap<EditAbsencesSummaryRequest, AbsencesSummary>();

            CreateMap<AbsencesSummary, RecalculateAbsencesSummaryResponse>();
        }
    }
}
