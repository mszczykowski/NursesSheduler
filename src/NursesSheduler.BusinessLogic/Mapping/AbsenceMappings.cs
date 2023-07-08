using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.Domain.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries.GetAbsences;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class AbsenceMappings : Profile
    {
        public AbsenceMappings()
        {
            CreateMap<AddAbsenceRequest, Absence>();
            CreateMap<Absence, AddAbsenceResponse>();

            CreateMap<Absence, GetAbsencesResponse>();

            CreateMap<Absence, GetAbsencesSummaryResponse.AbsenceResponse>();
        }
    }
}
