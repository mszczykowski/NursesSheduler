using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IScheduleSolverService
    {
        Task<IScheduleSolver> GetScheduleSolver(int year, int departamentId, Schedule currentSchedule, 
            CancellationToken cancellationToken);
    }
}