using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Veryfication
{
    internal sealed class ScheduleVeryficator
    {
        public void VerifySchedule(Quarter quarter, Schedule schedule, DepartamentSettings departamentSettings)
        {
            // przerwa
            // ilośc w tygodniu
            // ilość w miesiącu
            // ilość w kwartale

            foreach(var scheduleNurse in schedule.ScheduleNurses)
            {
            }
        }
    }
}
