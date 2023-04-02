using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class AbsencesService : IAbsencesService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentDateService _currentDateService;

        public AbsencesService(IApplicationDbContext context, ICurrentDateService currentDateService)
        {
            _context = context;
            _currentDateService = currentDateService;
        }

        public async Task<TimeSpan> CalculateAbsenceAssignedWorkingTime(Absence absence)
        {
            var assignedShifts = await _context.Shifts.Where(s => s.Date >= absence.From && s.Date <= absence.To &&
                                                           s.AssignedNurses.Any(n => n.NurseId == absence.YearlyAbsencesSummary.NurseId))
                                                                                                            .ToListAsync();

            var assignedWorkingTime = TimeSpan.Zero;

            foreach (var shift in assignedShifts)
            {
                assignedWorkingTime += shift.ShiftEnd - shift.ShiftStart;
            }

            return assignedWorkingTime;
        }

        public async Task InitializeDepartamentAbsencesSummary(Departament departament, CancellationToken cancellationToken)
        {
            var shouldBeInitializedToYear = _currentDateService.GetCurrentDate().Year + 1;

            var nurses = await _context.Nurses
                .Include(n => n.YearlyAbsencesSummaries)
                .Where(n => n.DepartamentId == departament.DepartamentId && n.IsDeleted == false)
                .ToListAsync();

            foreach (var nurse in nurses)
            {
                await InitializeNurseAbsencesSummary(nurse, departament, cancellationToken);
            }
        }

        public async Task InitializeNurseAbsencesSummary(Nurse nurse, Departament departament,
                                                                                    CancellationToken cancellationToken)
        {
            var currentYear = _currentDateService.GetCurrentDate().Year;

            var shouldBeInitializedToYear = _currentDateService.GetCurrentDate().Year + 1;

            for (int i = departament.CreationYear; i <= shouldBeInitializedToYear; i++)
            {
                if (!nurse.YearlyAbsencesSummaries.Any(n => n.Year == i))
                {
                    nurse.YearlyAbsencesSummaries.Add(
                        new AbsencesSummary
                        {
                            NurseId = nurse.NurseId,
                            Year = i,
                            PTOTime = nurse.PTOentitlement * TimeSpan.FromDays(1),
                        });
                }
            }

            var currentYearSummary = nurse.YearlyAbsencesSummaries
                                            .FirstOrDefault(y => y.Year == currentYear);

            var previousYearSummary = nurse.YearlyAbsencesSummaries
                                            .FirstOrDefault(y => y.Year == currentYear - 1);

            if (currentYearSummary != null && previousYearSummary != null)
            {
                currentYearSummary.PTOTimeLeftFromPreviousYear =
                    previousYearSummary.PTOTimeLeftFromPreviousYear + previousYearSummary.PTOTime
                    - previousYearSummary.PTOTimeUsed;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
