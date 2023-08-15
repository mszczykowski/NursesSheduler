using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament
{
    public class GetNursesFromDepartamentResponse
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DepartamentId { get; set; }
        public NurseTeams NurseTeam { get; set; }
        public TimeSpan NightHoursBalance { get; set; }
        public TimeSpan HolidayHoursBalance { get; set; }
        public bool IsDeleted { get; set; }
    }
}
