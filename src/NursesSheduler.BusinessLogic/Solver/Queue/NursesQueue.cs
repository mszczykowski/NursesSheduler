using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Queue
{
    internal sealed class NursesQueue : NursesQueueBase
    {
        private Queue<int> _nursesQueue;

        public NursesQueue(Random random) : base(random)
        {
            _nursesQueue = new Queue<int>();
        }

        public NursesQueue(NursesQueue queueToCopy) : base(queueToCopy)
        {
            _nursesQueue = new Queue<int>(queueToCopy._nursesQueue);
        }

        public override int GetQueueLenght()
        {
            return _nursesQueue.Count;
        }

        public override bool IsEmpty()
        {
            return _nursesQueue is null || _nursesQueue.Count == 0;
        }

        public override void PopulateQueue(ISolverState solverState, Day day)
        {
            _nursesQueue = _nurseQueueDirector
                    .BuildSortedNursesQueue(solverState.CurrentShift,
                    day.IsWorkDay,
                    solverState.GetPreviousDayDayShift(),
                    solverState.NurseStates,
                    solverState.CurrentDay);
        }

        public override bool TryDequeue(out int result, bool isFirstTry)
        {
            return _nursesQueue.TryDequeue(out result);
        }
    }
}
