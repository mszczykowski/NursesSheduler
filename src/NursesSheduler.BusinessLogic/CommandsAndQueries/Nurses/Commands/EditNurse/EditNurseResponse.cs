﻿using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.EditNurse
{
    public sealed class EditNurseResponse
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }
        public NurseTeams NurseTeam { get; set; }
    }
}
