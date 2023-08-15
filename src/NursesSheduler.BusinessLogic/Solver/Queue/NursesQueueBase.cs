using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Queue;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Queue
{
    internal abstract class NursesQueueBase : INursesQueue
    {
        protected readonly INursesQueueDirector _nurseQueueDirector;
        protected readonly Random _random;

        public NursesQueueBase(Random random)
        {
            _random = random;
            _nurseQueueDirector = new NursesQueueDirector(random);
        }

        public NursesQueueBase(NursesQueueBase queueToCopy)
        {
            _random = queueToCopy._random;
            _nurseQueueDirector = queueToCopy._nurseQueueDirector;
        }

        public abstract bool IsEmpty();
        public abstract void PopulateQueue(ISolverState solverState, Day day);
        public abstract bool TryDequeue(out int result, bool isFirstTry);
        public abstract int GetQueueLenght();
    }
}
