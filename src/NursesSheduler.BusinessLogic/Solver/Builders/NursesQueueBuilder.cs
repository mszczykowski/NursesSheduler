using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Enums;
using System.Text;

namespace NursesScheduler.BusinessLogic.Solver.Builders
{
    internal sealed class NursesQueueBuilder : INurseQueueBuilder
    {
        private StringBuilder _orderByBuilder;
        private readonly Random _random;
        
        private List<INurseState> _nurses;
        private List<INurseState> _nursesPrioritised;
        
        public NursesQueueBuilder(Random random)
        {
            _random = random;
        }

        public void InitializeBuilder(IEnumerable<INurseState> nurses)
        {
            _nurses = new List<INurseState>(nurses);
            _nursesPrioritised = new List<INurseState>();
            _orderByBuilder = new StringBuilder();
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
            _nursesPrioritised.AddRange(_nurses.Where(n => previousDayShift.Contains(n.NurseId)));

            _nurses.RemoveAll(n => previousDayShift.Contains(n.NurseId));

            return this;
        }

        public INurseQueueBuilder ProritiseWorkersOnTimeOff(int currentDay)
        {
            _nursesPrioritised.AddRange(_nurses.Where(n => n.TimeOff[currentDay - 1]));

            _nurses.RemoveAll(n => _nursesPrioritised.Any(np => np.NurseId == n.NurseId));

            return this;
        }

        public Queue<int> GetResult()
        {
            if (_orderByBuilder.Length > 0)
            {
                _orderByBuilder.Length--;
            }

            var orderBy = _orderByBuilder.ToString();

            var sortedNurses = GetSortedNursesIds(_nurses, orderBy).ToList();

            if (_nursesPrioritised != null)
            {
                sortedNurses.InsertRange(0, GetSortedNursesIds(_nursesPrioritised, orderBy));
            }

            return new Queue<int>(sortedNurses);
        }

        private IEnumerable<int> GetSortedNursesIds(IEnumerable<INurseState> nurses, string orderBy)
        {
            return from nurse in nurses
                   orderby _random.Next(), orderBy
                   select nurse.NurseId;
        }
    }
}
