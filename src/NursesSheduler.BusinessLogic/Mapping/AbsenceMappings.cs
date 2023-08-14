using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.Domain.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries.GetAbsences;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class AbsenceMappings : Profile
    {
        public AbsenceMappings()
        {
            CreateMap<Absence, AddAbsenceResponse.AbsenceResponse>();

            CreateMap<Absence, EditAbsenceResponse.AbsenceResponse>();

            CreateMap<Absence, GetAbsencesResponse>();

            CreateMap<Absence, GetAbsencesSummaryResponse.AbsenceResponse>();
        }
    }
}
