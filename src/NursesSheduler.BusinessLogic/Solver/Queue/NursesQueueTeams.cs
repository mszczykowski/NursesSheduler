using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Queue
{
    internal sealed class NursesQueueTeams : NursesQueueBase
    {
        private Queue<int> _nursesQueueTeamA;
        private Queue<int> _nursesQueueTeamB;
        private NurseTeams _teamToDequeue;

        public NursesQueueTeams(Random random) : base(random)
        {
            _teamToDequeue = (NurseTeams)_random.Next(ScheduleConstatns.NumberOfTeams);

            _nursesQueueTeamA = new Queue<int>();
            _nursesQueueTeamB = new Queue<int>();
        }

        public NursesQueueTeams(NursesQueueTeams queueToCopy) : base(queueToCopy)
        {
            _teamToDequeue = queueToCopy._teamToDequeue;

            _nursesQueueTeamA = new Queue<int>(queueToCopy._nursesQueueTeamA);
            _nursesQueueTeamB = new Queue<int>(queueToCopy._nursesQueueTeamB);
        }

        public override int GetQueueLenght()
        {
            return _nursesQueueTeamA.Count + _nursesQueueTeamB.Count;
        }

        public override bool IsEmpty()
        {
            return _nursesQueueTeamA is null
                    || _nursesQueueTeamB is null
                    || _nursesQueueTeamA.Count == 0 && _nursesQueueTeamB.Count == 0;
        }

        public override void PopulateQueue(ISolverState solverState, Day day)
        {
            _nursesQueueTeamA = _nurseQueueDirector
                    .BuildSortedNursesQueue(solverState.CurrentShift,
                    day.IsWorkDay,
                    solverState.GetPreviousDayDayShift(),
                    solverState.NurseStates.Where(n => n.NurseTeam == NurseTeams.A));

            _nursesQueueTeamB = _nurseQueueDirector
                .BuildSortedNursesQueue(solverState.CurrentShift,
                day.IsWorkDay,
                solverState.GetPreviousDayDayShift(),
                solverState.NurseStates.Where(n => n.NurseTeam == NurseTeams.B));
        }

        public override bool TryDequeue(out int result, bool isFirstTry)
        {
            if (isFirstTry)
            {
                _teamToDequeue = _teamToDequeue == NurseTeams.A ? NurseTeams.B : NurseTeams.A;
            }

            if ((_teamToDequeue == NurseTeams.A && _nursesQueueTeamA.Count != 0) || _nursesQueueTeamB.Count == 0)
            {
                return _nursesQueueTeamA.TryDequeue(out result) ? true : false;
            }

            return _nursesQueueTeamB.TryDequeue(out result) ? true : false;

        }
    }
}
