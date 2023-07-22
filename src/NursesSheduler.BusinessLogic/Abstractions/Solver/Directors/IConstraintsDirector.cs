using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface IConstraintsDirector
    {
        IEnumerable<IConstraint> GetAllConstraints(DepartamentSettings departamentSettings,
            IEnumerable<DayNumbered> monthDays);
    }
}
