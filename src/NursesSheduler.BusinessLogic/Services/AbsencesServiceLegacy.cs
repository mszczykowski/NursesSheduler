using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class AbsencesServiceLegacy : IAbsencesService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentDateService _currentDateService;

        public AbsencesServiceLegacy(IApplicationDbContext context, ICurrentDateService currentDateService)
        {
            _context = context;
            _currentDateService = currentDateService;
        }

        public async Task<IEnumerable<Absence>> GetNurseAbsencesInMonthAsync(int year, int month, int nurseId)
        {
            return await _context.AbsencesSummaries
                .Include(s => s.Absences)
                .Where(s => s.NurseId == nurseId && s.Year == year)
                .SelectMany(s => s.Absences)
                .Where(a => a.Month == month)
                .ToListAsync();
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

        public ICollection<Absence> GetAbsencesFromAddAbsenceRequest(DateOnly from, DateOnly to, int absencesSummaryId,
            AbsenceTypes type)
        {
            var result = new List<Absence>();

            var currentAbsence = new Absence(from.Month);

            result.Add(currentAbsence);

            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (date.Month != currentAbsence.Month)
                {
                    currentAbsence = new Absence(date.Month);
                    result.Add(currentAbsence);
                }

                currentAbsence.Days.Add(date.Day);
            }

            foreach(var absence in result)
            {
                absence.AbsencesSummaryId = absencesSummaryId;
                absence.Type = type;
            }
            
            return result;
        }

        public async Task<AbsenceVeryficationResult> VerifyAbsence(AbsencesSummary absencesSummary, Absence absence)
        {
            if (absencesSummary.Absences.Any(a => a.Month == absence.Month && a.Days.Intersect(absence.Days).Any()))
            {
                return AbsenceVeryficationResult.AbsenceAlreadyExists;
            }

            if (await _context.Schedules
                .Include(s => s.Quarter)
                .AnyAsync(s => s.Quarter.DepartamentId == absencesSummary.Nurse.DepartamentId &&
                    s.Year == absencesSummary.Year &&
                    s.Month == absence.Month &&
                    s.IsClosed))
            {
                return AbsenceVeryficationResult.ClosedMonth;
            }

            return AbsenceVeryficationResult.Valid;
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
