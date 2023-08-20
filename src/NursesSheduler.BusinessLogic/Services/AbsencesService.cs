using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class AbsencesService : IAbsencesService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentDateService _currentDateService;
        private readonly IWorkTimeService _workTimeService;

        public AbsencesService(IApplicationDbContext applicationDbContext, ICurrentDateService currentDateService, IWorkTimeService workTimeService)
        {
            _applicationDbContext = applicationDbContext;
            _currentDateService = currentDateService;
            _workTimeService = workTimeService;
        }

        public async Task<AbsencesSummary> RecalculateAbsencesSummary(int currentSummaryId)
        {
            var currentSummary = await _applicationDbContext.AbsencesSummaries
                .Include(s => s.Absences)
                .Include(s => s.Nurse)
                .FirstOrDefaultAsync(s => s.AbsencesSummaryId == currentSummaryId);

            var previousSummary = await _applicationDbContext.AbsencesSummaries
                .FirstOrDefaultAsync(s => s.NurseId == currentSummary.NurseId && s.Year == currentSummary.Year - 1);

            var recalculatedSummary = new AbsencesSummary();

            if (previousSummary is null)
            {
                recalculatedSummary.PTOTimeLeftFromPreviousYear = TimeSpan.Zero;
            }
            else
            {
                recalculatedSummary.PTOTimeLeftFromPreviousYear = previousSummary.PTOTimeLeftFromPreviousYear
                    + previousSummary.PTOTimeLeft;
            }

            recalculatedSummary.PTOTimeLeft = TimeSpan.FromDays(currentSummary.Nurse.PTOentitlement);

            SubtractAvailablePTOTime(recalculatedSummary, currentSummary.Absences);

            return recalculatedSummary;
        }

        public async Task AssignTimeOffsWorkTime(Schedule closedSchedule, int year, CancellationToken cancellationToken)
        {
            var absencesSummaries = await _applicationDbContext.AbsencesSummaries
                .Where(s => s.Year == year && closedSchedule.ScheduleNurses.Select(n => n.NurseId).Contains(s.NurseId))
                .ToListAsync();

            foreach(var absenceSummary in absencesSummaries)
            {
                if(absenceSummary.Absences is null)
                {
                    continue;
                }

                var absences = absenceSummary.Absences.Where(a => a.Month == closedSchedule.Month);

                foreach(var absence in absences)
                {
                    var assignedWorkTime = TimeSpan.Zero;

                    var absenceWorkDays = closedSchedule.ScheduleNurses
                        .First(n => n.NurseId == absenceSummary.NurseId)
                        .NurseWorkDays
                        .Where(wd => absence.Days.Contains(wd.Day));

                    foreach(var workday in absenceWorkDays)
                    {
                        assignedWorkTime += _workTimeService.GetAssignedShiftWorkTime(workday.ShiftType, 
                            workday.MorningShift?.ShiftLength);
                    }

                    absence.IsClosed = true;
                    absence.AssignedWorkingHours = assignedWorkTime;
                }

                SubtractAvailablePTOTime(absenceSummary, absences);
            }
        }


        public async Task<IEnumerable<Absence>> GetNurseAbsencesInMonthAsync(int year, int month, int nurseId)
        {
            return await _applicationDbContext.AbsencesSummaries
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

            var nurses = await _applicationDbContext.Nurses
                .Include(n => n.AbsencesSummaries)
                .Where(n => n.DepartamentId == departament.DepartamentId && n.IsDeleted == false)
                .ToListAsync();

            foreach (var nurse in nurses)
            {
                InitializeNurseAbsencesSummary(nurse, departament);
                RecalculatePreviousYearAbsencesSummary(nurse, departament);
            }

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public void InitializeNewNurseAbsencesSummaries(Nurse nurse, Departament departament)
        {
            nurse.AbsencesSummaries = new List<AbsencesSummary>();
            InitializeNurseAbsencesSummary(nurse, departament);
        }

        public IEnumerable<Absence> GetAbsencesFromDates(DateOnly from, DateOnly to, int absencesSummaryId,
            AbsenceTypes type)
        {
            Absence currentAbsence = null;

            var absences = new List<Absence>();

            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (!absences.Any(a => a.Month == date.Month))
                {
                    currentAbsence = new Absence
                    {
                        Month = date.Month,
                        Days = new List<int>(),
                    };
                    absences.Add(currentAbsence);
                }

                currentAbsence.Days.Add(date.Day);
            }

            foreach(var absence in absences)
            {
                absence.AbsencesSummaryId = absencesSummaryId;
                absence.Type = type;
            }
            
            return absences;
        }

        public async Task<AbsenceVeryficationResult> VerifyAbsence(AbsencesSummary absencesSummary, Absence absence)
        {
            if (absencesSummary.Absences.Any(a => a.Month == absence.Month && a.Days.Intersect(absence.Days).Any() 
                && a.AbsenceId != absence.AbsenceId))
            {
                return AbsenceVeryficationResult.AbsenceAlreadyExists;
            }

            if (await _applicationDbContext.Schedules
                .Include(s => s.Quarter)
                .AnyAsync(s => s.Quarter.DepartamentId == absencesSummary.Nurse.DepartamentId &&
                    s.Quarter.Year == absencesSummary.Year &&
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

            if (currentYearSummary is not null && previousYearSummary is not null)
            {
                currentYearSummary.PTOTimeLeftFromPreviousYear =
                    previousYearSummary.PTOTimeLeftFromPreviousYear + previousYearSummary.PTOTimeLeft;
            }
        }

        private void InitializeNurseAbsencesSummary(Nurse nurse, Departament departament)
        {
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
                            PTOTimeLeft = nurse.PTOentitlement * TimeSpan.FromDays(1),
                        });
                }
            }
        }

        private void SubtractAvailablePTOTime(AbsencesSummary absencesSummary, IEnumerable<Absence> absences)
        {
            foreach (var absence in absences)
            {
                if(absence.Type != AbsenceTypes.PersonalTimeOff)
                {
                    continue;
                }

                if(absencesSummary.PTOTimeLeftFromPreviousYear > TimeSpan.Zero)
                {
                    absencesSummary.PTOTimeLeftFromPreviousYear -= absence.AssignedWorkingHours;

                    if(absencesSummary.PTOTimeLeftFromPreviousYear < TimeSpan.Zero)
                    {
                        absencesSummary.PTOTimeLeft -= absencesSummary.PTOTimeLeftFromPreviousYear;
                        absencesSummary.PTOTimeLeftFromPreviousYear = TimeSpan.Zero;
                    }
                }
                else
                {
                    absencesSummary.PTOTimeLeft -= absence.AssignedWorkingHours;
                }
            }
        }
    }
}
