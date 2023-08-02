using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule
{
    public sealed class SolveScheduleRequest : IRequest<SolveScheduleResponse>
    {
        public Action SolverEventsListner;
        public int DepartamentId { get; set; }
        public int Year { get; set; }
        public ScheduleRequest Schedule { get; set; }
        public SolverSettingsRequest SolverSettings { get; set; }

        public sealed class ScheduleRequest
        {
            public int ScheduleId { get; set; }
            public int QuarterId { get; set; }
            public int Month { get; set; }
            public bool IsClosed { get; set; }
            public IEnumerable<ScheduleNurseRequest> ScheduleNurses { get; set; }
        }

        public sealed class ScheduleNurseRequest
        {
            public int NurseId { get; init; }
            public IEnumerable<NurseWorkDayRequest> NurseWorkDays { get; set; }
        }

        public sealed class NurseWorkDayRequest
        {
            public int Day { get; set; }
            public bool IsTimeOff { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public int? MorningShiftId { get; set; }
        }

        public sealed class SolverSettingsRequest
        {
            public int NumberOfRetries { get; set; }
            public bool UseOwnSeed { get; set; }
            public string? GeneratorSeed { get; set; }
        }
    }
}
