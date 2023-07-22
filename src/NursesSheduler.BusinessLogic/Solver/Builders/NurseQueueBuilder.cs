using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Enums;
using System.Linq.Dynamic.Core;
using System.Text;

namespace NursesScheduler.BusinessLogic.Solver.Builders
{
    internal sealed class NurseQueueBuilder : INurseQueueBuilder
    {
        private readonly StringBuilder _orderByBuilder;
        private readonly Random _random;
        
        private List<INurseState> _nurses;
        private List<INurseState> _nursesPrioritised;
        
        public NurseQueueBuilder(IEnumerable<INurseState> nurses, Random random)
        {
            _nurses = new List<INurseState>(nurses);
            _nursesPrioritised = new List<INurseState>();
            _orderByBuilder = new StringBuilder();
            _random = random;
        }

        public INurseQueueBuilder FilterTeam(NurseTeams nursesTeam)
        {
            _nurses = _nurses.Where(n => n.NurseTeam == nursesTeam).ToList();
            _nursesPrioritised = _nursesPrioritised.Where(n => n.NurseTeam == nursesTeam).ToList();

            return this;
        }

        public INurseQueueBuilder OrderByLongestBreak()
        {
            _orderByBuilder.Append(nameof(INurseState.HoursFromLastShift) + " desc,");

            return this;
        }

        public INurseQueueBuilder OrderByLowestNumberOfHolidayShitfs()
        {
            _orderByBuilder.Append(nameof(INurseState.HolidayHoursAssigned) + ",");

            return this;
        }

        public INurseQueueBuilder OrderByLowestNumberOfNightShitfs()
        {
            _orderByBuilder.Append(nameof(INurseState.NightHoursAssigned) + ",");

            return this;
        }

        public INurseQueueBuilder ProritisePreviousDayShiftWorkers(HashSet<int> previousDayShift)
        {
            _nursesPrioritised = _nurses.Where(n => previousDayShift.Contains(n.NurseId)).ToList();

            _nurses.RemoveAll(n => previousDayShift.Contains(n.NurseId));

            return this;
        }

        public Queue<int> GetResult()
        {
            //remove last coma
            if (_orderByBuilder.Length > 0)
            {
                _orderByBuilder.Length--;
            }

            var orderBy = _orderByBuilder.ToString();

            var sortedNurses = _nurses.AsQueryable().OrderBy(a => _random.Next()).OrderBy(orderBy).ToList();

            if (_nursesPrioritised != null)
            {
                sortedNurses.InsertRange(0, _nursesPrioritised.AsQueryable().OrderBy(orderBy).ToList());
            }

            var queue = new Queue<int>();
            sortedNurses.ForEach(n => queue.Enqueue(n.NurseId));

            return queue;
        }
    }
}
