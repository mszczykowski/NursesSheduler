using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver;
using NursesScheduler.BusinessLogic.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SolverService : ISolverService
    {
        public ISolverState? SolveSchedule(Schedule currentSchedule, Schedule previousSchedule, 
            DepartamentSettings departamentSettings, ICollection<IConstraint> constraints, 
            ICollection<NurseQuarterStats> nurseQuarterStats, Random random)
        {
            var nurseStates = new HashSet<INurseState>();

            foreach (var quarterStats in nurseQuarterStats)
            {
                nurseStates.Add(new NurseState(quarterStats,
                    previousSchedule.ScheduleNurses.FirstOrDefault(n => n.NurseId == quarterStats.NurseId),
                    currentSchedule.ScheduleNurses.First(n => n.NurseId == quarterStats.NurseId)));
            }

            var initialSolverState = new SolverState(currentSchedule, nurseStates);

            var currentMonthDays = currentSchedule.MonthDays.OrderBy(d => d.Date).ToArray();

            var solver = new ScheduleSolver(currentSchedule.Quarter.MorningShifts, currentMonthDays, constraints,
                departamentSettings);

            var shiftCapacityManager = new ShiftCapacityManager(departamentSettings, currentMonthDays,
                    nurseQuarterStats.Count, currentSchedule.WorkTimeInMonth, random);

            return solver.GenerateSchedule(random, shiftCapacityManager, initialSolverState);
        }
    }
}
