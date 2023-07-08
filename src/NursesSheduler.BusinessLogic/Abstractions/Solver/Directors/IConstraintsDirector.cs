using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface IConstraintsDirector
    {
        ICollection<IConstraint> GetAllConstraints(DepartamentSettings departamentSettings);
    }
}
