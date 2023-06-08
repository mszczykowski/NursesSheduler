using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IAbsencesService
    {
        Task<TimeSpan> CalculateAbsenceAssignedWorkingTime(Absence absence);
        Task InitializeDepartamentAbsencesSummaries(Departament departament, CancellationToken cancellationToken);
        void InitializeNewNurseAbsencesSummaries(Nurse nurse, Departament departament);
        ICollection<Absence> GetAbsencesFromAddAbsenceRequest(AddAbsenceRequest absenceRequest);
        Task<AbsenceVeryficationResult> VerifyAbsence(AbsencesSummary absencesSummary, Absence absence)
    }
}