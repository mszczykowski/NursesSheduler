using System.Linq.Dynamic.Core;
using System.Text;
using ScheduleSolver.Implementation.StateManagers;
using ScheduleSolver.Interfaces.Builders;
using ScheduleSolver.Interfaces.StateManagers;

namespace ScheduleSolver.Implementation.Builders
{
    internal sealed class EmployeeQueueBuilder : IEmployeeQueueBuilder
    {
        private List<IEmployeeState> employees;
        private List<IEmployeeState> employeesPrioritised;
        private StringBuilder orderByBuilder;

        private readonly Random _random;

        public EmployeeQueueBuilder(List<IEmployeeState> employees, Random random)
        {
            this.employees = new List<IEmployeeState>(employees);

            orderByBuilder = new StringBuilder();

            _random = random;
        }

        public IEmployeeQueueBuilder RemoveEmployeesOnPTO(int dayNumber)
        {
            employees = employees.Where(e => e.PTO[dayNumber - 1] == false).ToList();
            if (employeesPrioritised != null) employeesPrioritised = employeesPrioritised
                    .Where(e => e.PTO[dayNumber - 1] == false).ToList();

            return this;
        }

        public IEmployeeQueueBuilder OrderByLongestBreak()
        {
            orderByBuilder.Append(nameof(EmployeeState.DaysFromLastShift) + " desc,");

            return this;
        }

        public IEmployeeQueueBuilder OrderByLowestNumberOfHolidayShitfs()
        {
            orderByBuilder.Append(nameof(EmployeeState.HolidayPaidHoursAssigned) + ",");

            return this;
        }

        public IEmployeeQueueBuilder OrderByLowestNumberOfNightShitfs()
        {
            orderByBuilder.Append(nameof(EmployeeState.NumberOfNightShifts) + ",");

            return this;
        }

        public IEmployeeQueueBuilder ProritisePreviousDayShiftWorkers(List<int> previousDayShift)
        {
            IEmployeeState tmp;

            employeesPrioritised = new List<IEmployeeState>();

            foreach (var id in previousDayShift)
            {
                tmp = employees.FirstOrDefault(e => e.Id == id);

                if (tmp != null)
                {
                    employeesPrioritised.Add(tmp);
                    employees.Remove(tmp);
                }
            }

            return this;
        }

        public Queue<int> GetResult()
        {
            if (orderByBuilder.Length > 0) orderByBuilder = orderByBuilder.Remove(orderByBuilder.Length - 1, 1);

            var orderBy = orderByBuilder.ToString();

            var result = employees.AsQueryable().OrderBy(a => _random.Next()).OrderBy(orderBy).ToList();

            if (employeesPrioritised != null)
            {
                result.InsertRange(0, employeesPrioritised.AsQueryable().OrderBy(orderBy).ToList());
            }

            var queue = new Queue<int>();
            result.ForEach(r => queue.Enqueue(r.Id));

            return queue;
        }
    }
}
