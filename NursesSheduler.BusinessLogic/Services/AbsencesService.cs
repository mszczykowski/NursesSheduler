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
        private readonly IWorkTimeService _workTimeService;

        public AbsencesService(IApplicationDbContext context, ICurrentDateService currentDateService,
            IWorkTimeService workTimeService)
        {
            _context = context;
            _currentDateService = currentDateService;
            _workTimeService = workTimeService;
        }

        public async Task<TimeSpan> CalculateAbsenceAssignedWorkingTime(Absence absence)
        {
            var absenceWorkDays = absence.NurseWorkDays;

            return _workTimeService.GetWorkingTimeFromWorkDays(absenceWorkDays);
        }

        public async Task InitializeDepartamentAbsencesSummaries(Departament departament, 
            CancellationToken cancellationToken)
        {
            var shouldBeInitializedToYear = _currentDateService.GetCurrentDate().Year + 1;

            var nurses = await _context.Nurses
                .Include(n => n.AbsencesSummaries)
                .Where(n => n.DepartamentId == departament.DepartamentId && n.IsDeleted == false)
                .ToListAsync();

            foreach (var nurse in nurses)
            {
                InitializeNurseAbsencesSummary(nurse, departament);
                RecalculatePreviousYearAbsencesSummary(nurse, departament);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public void InitializeNewNurseAbsencesSummaries(Nurse nurse, Departament departament)
        {
            nurse.AbsencesSummaries = new List<AbsencesSummary>();
            InitializeNurseAbsencesSummary(nurse, departament);
        }

        private void RecalculatePreviousYearAbsencesSummary(Nurse nurse, Departament departament)
        {
            var currentYear = _currentDateService.GetCurrentDate().Year;

            var currentYearSummary = nurse.AbsencesSummaries
                                            .FirstOrDefault(y => y.Year == currentYear);

            var previousYearSummary = nurse.AbsencesSummaries
                                            .FirstOrDefault(y => y.Year == currentYear - 1);

            if (currentYearSummary != null && previousYearSummary != null)
            {
                currentYearSummary.PTOTimeLeftFromPreviousYear =
                    previousYearSummary.PTOTimeLeftFromPreviousYear + previousYearSummary.PTOTime
                    - previousYearSummary.PTOTimeUsed;
            }
        }

        private void InitializeNurseAbsencesSummary(Nurse nurse, Departament departament)
        {
            var currentYear = _currentDateService.GetCurrentDate().Year;

            var shouldBeInitializedToYear = _currentDateService.GetCurrentDate().Year + 1;

            for (int i = departament.CreationYear; i <= shouldBeInitializedToYear; i++)
            {
                if (!nurse.AbsencesSummaries.Any(n => n.Year == i))
                {
                    nurse.AbsencesSummaries.Add(
                        new AbsencesSummary
                        {
                            NurseId = nurse.NurseId,
                            Year = i,
                            PTOTime = nurse.PTOentitlement * TimeSpan.FromDays(1),
                        });
                }
            }
        }
    }
}
