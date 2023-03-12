using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Interfaces;

namespace NursesScheduler.Domain.Models.Settings.Constraints
{
    public sealed class BreakConstraint : IConstraint
    {
        public bool IsEnabled { get; set; }
        public TimeSpan MinimalBreak { get; set; }

        public int DepartamentId { get; set; }
        public Departament Departament { get; set; }
    }
}
