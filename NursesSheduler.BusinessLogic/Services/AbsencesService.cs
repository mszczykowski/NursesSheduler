using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

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

        public ICollection<Absence> GetAbsencesFromAddAbsenceRequest(AddAbsenceRequest absenceRequest)
        {
            var result = new List<Absence>();

            var currentAbsence = new Absence(absenceRequest.From.Month);

            result.Add(currentAbsence);

            for (var date = absenceRequest.From; date <= absenceRequest.To; date = date.AddDays(1))
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
                absence.AbsencesSummaryId = absenceRequest.AbsencesSummaryId;
                absence.Type = absenceRequest.Type;
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
                .AnyAsync(s => s.DepartamentId == absencesSummary.Nurse.DepartamentId &&
                    s.Year == absencesSummary.Year &&
                    s.MonthNumber == absence.Month &&
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
