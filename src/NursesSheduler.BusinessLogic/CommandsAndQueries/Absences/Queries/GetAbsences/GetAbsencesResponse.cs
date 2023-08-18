using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries.GetAbsences
{
    public sealed class GetAbsencesResponse
    {
        public int AbsenceId { get; set; }
        public int Month { get; set; }
        public ICollection<int> Days { get; set; }
        public TimeSpan WorkingHoursToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }
        public bool IsClosed { get; set; }
        public int AbsencesSummaryId { get; set; }
    }
}
