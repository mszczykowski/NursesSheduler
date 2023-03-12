using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Interfaces;

namespace NursesScheduler.Domain.Models.Settings.Constraints
{
    public sealed class MaxTotalHoursInWeekConstraint : IConstraint
    {
        public bool IsEnabled { get; set; }
        public TimeSpan MaxTotalHoursInWeek { get; set; }

        public int DepartamentId { get; set; }
        public Departament Departament { get; set; }
    }
}
