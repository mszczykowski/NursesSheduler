using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNurse
{
    public sealed class GetNurseResponse
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }
        public NurseTeams NurseTeam { get; set; }
        public int DepartamentId { get; set; }
    }
}
