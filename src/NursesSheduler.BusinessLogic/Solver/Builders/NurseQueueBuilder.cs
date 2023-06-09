using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
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
        
        public NurseQueueBuilder(ICollection<INurseState> nurses, Random random)
        {
            _nurses = new List<INurseState>(nurses);
            _orderByBuilder = new StringBuilder();
            _random = random;
        }

        public INurseQueueBuilder RemoveEmployeesOnPTO(int dayNumber)
        {
            _nurses = _nurses.Where(e => e.TimeOff[dayNumber - 1] == false).ToList();
            
            if (_nursesPrioritised != null) _nursesPrioritised = _nursesPrioritised
                    .Where(e => e.TimeOff[dayNumber - 1] == false).ToList();

            return this;
        }

        public INurseQueueBuilder OrderByLongestBreak()
        {
            _orderByBuilder.Append(nameof(INurseState.HoursFromLastShift) + " desc,");

            return this;
        }

        public INurseQueueBuilder OrderByLowestNumberOfHolidayShitfs()
        {
            _orderByBuilder.Append(nameof(INurseState.HolidayPaidHoursAssigned) + ",");

            return this;
        }

        public INurseQueueBuilder OrderByLowestNumberOfNightShitfs()
        {
            _orderByBuilder.Append(nameof(INurseState.NumberOfNightShifts) + ",");

            return this;
        }

        public INurseQueueBuilder ProritisePreviousDayShiftWorkers(List<int> previousDayShift)
        {
            _nursesPrioritised = new List<INurseState>(_nurses.Where(n => previousDayShift.Contains(n.NurseId)).ToList());
            _nurses.RemoveAll(n => previousDayShift.Contains(n.NurseId));

            return this;
        }

        public Queue<int> GetResult()
        {
            if (_orderByBuilder.Length > 0) 
                _orderByBuilder.Length--;

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
