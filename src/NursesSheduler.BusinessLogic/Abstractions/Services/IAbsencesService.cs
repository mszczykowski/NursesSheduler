using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IAbsencesService
    {
        Task InitializeDepartamentAbsencesSummaries(Departament departament, CancellationToken cancellationToken);
        void InitializeNewNurseAbsencesSummaries(Nurse nurse, Departament departament);
        ICollection<Absence> GetAbsencesFromAddAbsenceRequest(DateOnly from, DateOnly to, int absencesSummaryId,
            AbsenceTypes type);
        Task<AbsenceVeryficationResult> VerifyAbsence(AbsencesSummary absencesSummary, Absence absence);
        Task<IEnumerable<Absence>> GetNurseAbsencesInMonthAsync(int year, int month, int nurseId);
        Task AssignTimeOffsWorkTime(Schedule closedSchedule, int year, CancellationToken cancellationToken);
        Task<AbsencesSummary> RecalculateAbsencesSummary(int currentSummaryId);
    }
}