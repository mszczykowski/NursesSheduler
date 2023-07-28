using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule
{
    public sealed class SolveScheduleResponse
    {
        public IEnumerable<ScheduleNurseResponse> ScheduleNurses { get; set; }
        public SolverSettingsResponse SolverSettings { get; set; }

        public sealed class ScheduleNurseResponse
        {
            public int NurseId { get; init; }
            public IEnumerable<NurseWorkDayRepsonse> NurseWorkDays { get; set; }
        }

        public sealed class NurseWorkDayRepsonse
        {
            public int Day { get; set; }
            public bool IsTimeOff { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public int MorningShiftId { get; set; }
        }

        public sealed class SolverSettingsResponse
        {
            public int NumberOfRetries { get; set; }
            public bool UseOwnSeed { get; set; }
            public string? GeneratorSeed { get; set; }
        }
    }
}
