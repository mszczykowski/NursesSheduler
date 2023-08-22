using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.States;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.Builders
{
    internal sealed class NurseStateBuilder : INurseStateBuilder
    {
        private int _nurseId;
        private Dictionary<int, TimeSpan> _workTimeAssignedInWeeks;
        private TimeSpan _hoursFromLastShift;
        private TimeSpan[] _hoursToNextShiftMatrix;
        private int _numberOfRegularShiftsToAssign;
        private int _numberOfTimeOffShiftsToAssign;
        private TimeSpan _holidayHoursAssigned;
        private TimeSpan _nightHoursAssigned;
        private TimeSpan _workTimeInQuarterLeft;
        private bool[] _timeOff;
        private HashSet<int> _previouslyAssignedMorningShifts;
        private int? _assignedMorningShiftId;
        private ShiftTypes _previousMonthLastShift;
        private NurseTeams _nurseTeam;

        public INurseStateBuilder SetNurseId(ScheduleNurse scheduleNurse)
        {
            _nurseId = scheduleNurse.NurseId;
            return this;
        }

        public INurseStateBuilder SetWorkTimeAssignedInWeeks(NurseStats nurseQuarterStats)
        {
            _workTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(nurseQuarterStats.WorkTimeAssignedInWeeks);
            return this;
        }

        public INurseStateBuilder SetHoursFromLastShift(int previousMonthLength,
            NurseScheduleStats? previousScheduleNurseStats)
        {
            _hoursFromLastShift = previousScheduleNurseStats?.HoursFromLastAssignedShift
                ?? TimeSpan.FromDays(previousMonthLength);
            return this;
        }

        public INurseStateBuilder SetHoursToNextShiftMatrix(int nextMonthLength, IEnumerable<NurseWorkDay> nurseWorkDays,
            NurseScheduleStats? nextScheduleNurseStats, IWorkTimeService workTimeService)
        {
            _hoursToNextShiftMatrix = new TimeSpan[nurseWorkDays.Count() + 1];

            for (int i = 0; i < nurseWorkDays.Count(); i++)
            {
                _hoursToNextShiftMatrix[i] = workTimeService.GetHoursToFirstAssignedShift(i + 1, nurseWorkDays);

                if (!nurseWorkDays.Any(wd => wd.Day >= i + 1 && wd.ShiftType != ShiftTypes.None))
                {
                    _hoursToNextShiftMatrix[i] += GetNextScheudleTimeToFirstShift(nextMonthLength,
                        nextScheduleNurseStats);
                }
            }

            _hoursToNextShiftMatrix[_hoursToNextShiftMatrix.Length - 1] = GetNextScheudleTimeToFirstShift(nextMonthLength,
                nextScheduleNurseStats);

            return this;
        }

        public INurseStateBuilder SetNumbersOfShifts(int numberOfShiftsToAssignInMonth, 
            NurseScheduleStats nurseScheduleStats, IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            _numberOfRegularShiftsToAssign = numberOfShiftsToAssignInMonth;

            _numberOfTimeOffShiftsToAssign = (int)Math.Round(nurseScheduleStats.TimeOffToAssign 
                / ScheduleConstatns.RegularShiftLength);

            _numberOfTimeOffShiftsToAssign -= nurseWorkDays
                .Count(wd => wd.IsTimeOff && wd.ShiftType != ShiftTypes.None && wd.ShiftType != ShiftTypes.Morning);

            if (_numberOfTimeOffShiftsToAssign < 0)
            {
                _numberOfRegularShiftsToAssign += _numberOfTimeOffShiftsToAssign;
                _numberOfTimeOffShiftsToAssign = 0;
            }
            _numberOfRegularShiftsToAssign -= _numberOfTimeOffShiftsToAssign;

            _numberOfRegularShiftsToAssign -= nurseWorkDays
                .Count(wd => !wd.IsTimeOff && wd.ShiftType != ShiftTypes.None && wd.ShiftType != ShiftTypes.Morning);

            return this;
        }

        public INurseStateBuilder SetSpecialWorkHours(Nurse nurse, IEnumerable<ScheduleStats> schedulesStats)
        {
            _holidayHoursAssigned = nurse.HolidayHoursBalance
                + schedulesStats
                    .Where(s => !s.IsClosed)
                    .SelectMany(s => s.NursesScheduleStats.Where(ns => ns.NurseId == nurse.NurseId))
                    .Select(ns => ns.HolidayHoursAssigned)
                    .SumTimeSpan();

            _nightHoursAssigned = nurse.NightHoursBalance
                + schedulesStats
                    .Where(s => !s.IsClosed)
                    .SelectMany(s => s.NursesScheduleStats.Where(ns => ns.NurseId == nurse.NurseId))
                    .Select(ns => ns.NightHoursAssigned)
                    .SumTimeSpan();

            return this;
        }

        public INurseStateBuilder SetWorkTimeInQuarterLeft(TimeSpan workTimeInQuarter, NurseStats nurseQuarterStats)
        {
            _workTimeInQuarterLeft = workTimeInQuarter - nurseQuarterStats.AssignedWorkTime;

            return this;
        }

        public INurseStateBuilder SetTimeOffs(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            _timeOff = new bool[nurseWorkDays.Count()];

            foreach (var nurseWorkDay in nurseWorkDays.Where(wd => wd.IsTimeOff))
            {
                _timeOff[nurseWorkDay.Day - 1] = true;
            }

            return this;
        }

        public INurseStateBuilder SetPreviouslyAssignedMorningShifts(NurseStats nurseQuarterStats)
        {
            _previouslyAssignedMorningShifts = new HashSet<int>(nurseQuarterStats.AssignedMorningShiftsIds);

            return this;
        }

        public INurseStateBuilder SetAssignedMorningShift(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            _assignedMorningShiftId = nurseWorkDays
                .FirstOrDefault(wd => wd.ShiftType == ShiftTypes.Morning)?
                .MorningShift?
                .MorningShiftId;

            return this;
        }

        public INurseStateBuilder SetPreviousMonthLastShift(NurseScheduleStats? previousScheduleNurseStats)
        {
            _previousMonthLastShift = previousScheduleNurseStats?.LastState ?? ShiftTypes.None;

            return this;
        }

        public INurseStateBuilder SetNurseTeam(Nurse nurse)
        {
            _nurseTeam = nurse.NurseTeam;

            return this;
        }

        public INurseState GetResult()
        {
            return new NurseState
            {
                NurseId = _nurseId,
                WorkTimeAssignedInWeeks = _workTimeAssignedInWeeks,
                HoursFromLastShift = _hoursFromLastShift,
                HoursToNextShiftMatrix = _hoursToNextShiftMatrix,
                NumberOfRegularShiftsToAssign = _numberOfRegularShiftsToAssign,
                NumberOfTimeOffShiftsToAssign = _numberOfTimeOffShiftsToAssign,
                HolidayHoursAssigned = _holidayHoursAssigned,
                NightHoursAssigned = _nightHoursAssigned,
                WorkTimeInQuarterLeft = _workTimeInQuarterLeft,
                TimeOff = _timeOff,
                PreviouslyAssignedMorningShifts = _previouslyAssignedMorningShifts,
                AssignedMorningShiftId = _assignedMorningShiftId,
                PreviousMonthLastShift = _previousMonthLastShift,
                NurseTeam = _nurseTeam,
            };
        }


        private TimeSpan GetNextScheudleTimeToFirstShift(int nextMonthLength, NurseScheduleStats? nextScheduleNurseStats)
        {
            return nextScheduleNurseStats?.HoursToFirstAssignedShift ?? TimeSpan.FromDays(nextMonthLength);
        }
    }
}
