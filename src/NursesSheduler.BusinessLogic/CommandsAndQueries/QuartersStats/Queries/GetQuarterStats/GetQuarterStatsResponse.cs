namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats
{
    public sealed class GetQuarterStatsResponse
    {
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public IEnumerable<NurseStatsResponse> NurseStats { get; set; }

        public sealed class NurseStatsResponse
        {
            public int NurseId { get; set; }
            public TimeSpan AssignedWorkTime { get; set; }
            public TimeSpan HolidayHoursAssigned { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan TimeOffAssigned { get; set; }
            public int NightShiftsAssigned { get; set; }
        }
    }
}
