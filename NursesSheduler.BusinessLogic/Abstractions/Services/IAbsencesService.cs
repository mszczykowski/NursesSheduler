using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IAbsencesService
    {
        Task<TimeSpan> CalculateAbsenceAssignedWorkingTime(Absence absence);
        Task InitializeDepartamentAbsencesSummary(Departament departament, CancellationToken cancellationToken);
        Task InitializeNurseAbsencesSummary(Nurse nurse, Departament departament, CancellationToken cancellationToken);
    }
}