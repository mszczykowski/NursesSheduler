using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SchedulesService : ISchedulesService
    {
        private readonly IApplicationDbContext _context;
        private readonly IWorkTimeService _workTimeService;

        public SchedulesService(IApplicationDbContext context, IWorkTimeService workTimeService)
        {
            _context = context;
            _workTimeService = workTimeService;
        }

        public async Task<Schedule> GetNewSchedule(int monthNumber, int yearNumber, int departamentId, 
            DepartamentSettings departamentSettings)
        {
            var quarterNumber = await _workTimeService.GetQuarterNumber(monthNumber, departamentSettings);

            var nurses = await _context.Nurses
                .Where(n => n.IsDeleted == false && n.DepartamentId == departamentId)
                .ToListAsync();

            var schedule = new Schedule
            {
                MonthNumber = monthNumber,
                Year = yearNumber,
                DepartamentId = departamentId,
                TimeOffAssigned = TimeSpan.Zero,
            };

            schedule.Quarter = await GetQuarter(yearNumber, quarterNumber, departamentId, departamentSettings, nurses);

            schedule.WorkTimeInMonth = await _workTimeService
                .GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentSettings);

            schedule.TimeOffAvailableToAssgin = await _workTimeService
                .GetSurplusWorkTime(monthNumber, yearNumber, nurses.Count, departamentSettings);

            InitialiseScheduleNurses(nurses, schedule, DateTime.DaysInMonth(yearNumber, monthNumber));

            return schedule;
        }

        public async Task SetTimeOffs(Schedule schedule, DepartamentSettings departamentSettings)
        {
            var absenceSummaries = await _context.AbsencesSummaries
                .Where(a => a.Year == schedule.Year &&
                    a.Nurse.DepartamentId == schedule.DepartamentId)
                .Include(a => a.Absences)
                .ToListAsync();

            if (absenceSummaries == null || !absenceSummaries.Any())
                return;

            foreach(var scheduleNurse in schedule.ScheduleNurses)
            {
                var absences = absenceSummaries
                    .FirstOrDefault(a => a.NurseId == scheduleNurse.NurseId && a.Year == schedule.Year)?
                    .Absences?.Where(a => a.Month == schedule.MonthNumber)
                    .ToList();

                if (absences == null)
                    continue;

                foreach(var absence in absences)
                {
                    foreach(var day in absence.Days)
                    {
                        scheduleNurse.NurseWorkDays.First(d => d.DayNumber == day).IsTimeOff = true;
                    }
                    scheduleNurse.TimeOffToAssign += absence.WorkTimeToAssign;
                }
            }
        }

        private void InitialiseScheduleNurses(ICollection<Nurse> nurses, Schedule schedule, int numberOfDaysInMonth)
        {
            schedule.ScheduleNurses = new List<ScheduleNurse>();

            foreach (var nurse in nurses)
            {
                var scheduleNurse = new ScheduleNurse
                {
                    NurseId = nurse.NurseId,
                    Nurse = nurse,
                    NurseWorkDays = new List<NurseWorkDay>(),
                    TimeToAssingInMonth = schedule.WorkTimeInMonth,
                };

                for (int i = 0; i < numberOfDaysInMonth; i++)
                {
                    scheduleNurse.NurseWorkDays.Add(new NurseWorkDay
                    {
                        DayNumber = i + 1,
                        ShiftType = Domain.Enums.ShiftTypes.None
                    });
                }
                schedule.ScheduleNurses.Add(scheduleNurse);
            }
        }

        private async Task<Quarter> GetQuarter(int yearNumber, int quarterNumber, int departamentId,
            DepartamentSettings departamentSettings, ICollection<Nurse> nurses)
        {
            var quarter = await _context.Quarters
                .Include(q => q.NurseQuarterStats)
                .Include(q => q.MorningShifts)
                .FirstOrDefaultAsync(q => q.DepartamentId == departamentId
                                        && q.SettingsVersion == departamentSettings.SettingsVersion
                                        && q.QuarterNumber == quarterNumber
                                        && q.QuarterYear == yearNumber);

            if (quarter == null)
            {
                quarter = await CreateNewQuarter(yearNumber, quarterNumber, departamentId, departamentSettings);
            }

            await InitializeNurseQuarterStats(quarter, nurses, departamentSettings);

            return quarter;
        }

        private async Task<Quarter> CreateNewQuarter(int yearNumber, int quarterNumber, int departamentId,
            DepartamentSettings settings)
        {
            var quarter = new Quarter
            {
                QuarterYear = yearNumber,
                QuarterNumber = quarterNumber,
                DepartamentId = departamentId,
                SettingsVersion = settings.SettingsVersion,
                MorningShiftsReadOnly = false,
            };
            quarter.MorningShifts = new List<MorningShift>();
            quarter.NurseQuarterStats = new List<NurseQuarterStats>();
            quarter.WorkTimeInQuarterToAssign = await _workTimeService
                        .GetTotalWorkingHoursInQuarter(quarter.QuarterNumber, quarter.QuarterYear, settings);

            return quarter;
        }

        private async Task InitializeNurseQuarterStats(Quarter quarter, ICollection<Nurse> nurses, 
            DepartamentSettings departamentSettings)
        {
            bool statsChanged = false;
            TimeSpan workTimeToAssignInQuarter = TimeSpan.Zero;

            foreach (var nurse in nurses)
            {
                if (quarter.NurseQuarterStats.Any(q => q.NurseId == nurse.NurseId))
                    continue;

                if (workTimeToAssignInQuarter == TimeSpan.Zero)
                {
                    workTimeToAssignInQuarter = await _workTimeService
                        .GetTotalWorkingHoursInQuarter(quarter.QuarterNumber, quarter.QuarterYear, departamentSettings);
                }

                quarter.NurseQuarterStats.Add(new NurseQuarterStats
                {
                    WorkTimeInQuarterToAssign = workTimeToAssignInQuarter,
                    WorkTimeInQuarterToAssigned = TimeSpan.Zero,
                    HolidayPaidHoursAssigned = TimeSpan.Zero,
                    NumberOfNightShifts = 0,
                    NurseId = nurse.NurseId,
                });

                statsChanged = true;
            }

            if (statsChanged)
                await _context.SaveChangesAsync(default);
        }

        public async Task SetNurseWorkTimes(Schedule currentSchedule)
        {
            var previousSchedules = await _context.Schedules
                .Where(s => s.Quarter == currentSchedule.Quarter &&
                    s.MonthNumber != currentSchedule.MonthNumber &&
                    s.Year == currentSchedule.Year &&
                    s.DepartamentId == currentSchedule.DepartamentId)
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .OrderBy(s => s.MonthInQuarter)
                .ToListAsync();

            foreach(var nurse in currentSchedule.ScheduleNurses)
            {
                nurse.TimeToAssingInQuarterLeft = currentSchedule.Quarter.WorkTimeInQuarterToAssign;
            }

            for(int i = 0; i < previousSchedules.Count; i++)
            {
                foreach (var nurse in previousSchedules[i].ScheduleNurses)
                {
                    nurse.TimeToAssingInMonth = previousSchedules[i].WorkTimeInMonth - 
                        _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);

                    var currentNurseStats = currentSchedule.ScheduleNurses
                        .FirstOrDefault(n => n.NurseId == nurse.NurseId);

                    if (currentNurseStats != null)
                        currentNurseStats.TimeToAssingInQuarterLeft -= 
                            _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);

                    ScheduleNurse? prevNurseStats = null;

                    if(i - 1 >= 0)
                    {
                        prevNurseStats = previousSchedules[i - 1].ScheduleNurses
                            .FirstOrDefault(nurse => nurse.NurseId == nurse.NurseId);
                    }

                    if (prevNurseStats == null)
                    {
                        nurse.PreviousMonthTime = TimeSpan.Zero;
                    }
                    else
                    {
                        nurse.PreviousMonthTime = prevNurseStats.TimeToAssingInMonth + prevNurseStats.PreviousMonthTime;
                    }

                }
            }

            foreach (var nurse in currentSchedule.ScheduleNurses)
            {
                nurse.TimeToAssingInMonth = currentSchedule.WorkTimeInMonth - 
                    _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);
                
                nurse.TimeToAssingInQuarterLeft -= _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);

                ScheduleNurse? prevNurseStats = null;

                if (!previousSchedules.Any())
                    continue;

                prevNurseStats = previousSchedules.Last()?.ScheduleNurses
                    .FirstOrDefault(nurse => nurse.NurseId == nurse.NurseId);

                if (prevNurseStats == null)
                {
                    nurse.PreviousMonthTime = TimeSpan.Zero;
                }
                else
                {
                    nurse.PreviousMonthTime = prevNurseStats.TimeToAssingInMonth + prevNurseStats.PreviousMonthTime;
                }

            }
        }

        public void ValidateSchedule()
        {

        }
    }
}
