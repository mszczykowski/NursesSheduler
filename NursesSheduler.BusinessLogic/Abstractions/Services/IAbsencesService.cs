using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IAbsencesService
    {
        Task<TimeSpan> CalculateAbsenceAssignedWorkingTime(Absence absence);
        Task InitializeDepartamentAbsencesSummaries(Departament departament, CancellationToken cancellationToken);
        void InitializeNewNurseAbsencesSummaries(Nurse nurse, Departament departament);
    }
}