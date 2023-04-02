using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class AbsenceMappings : Profile
    {
        public AbsenceMappings()
        {
            CreateMap<AddAbsenceRequest, Absence>();
            CreateMap<Absence, AddAbsenceResponse>();

            CreateMap<EditAbsenceRequest, Absence>();
            CreateMap<Absence, EditAbsenceResponse>();

            CreateMap<Absence, GetAbsencesSummaryResponse.AbsenceResponse>();
        }
    }
}
