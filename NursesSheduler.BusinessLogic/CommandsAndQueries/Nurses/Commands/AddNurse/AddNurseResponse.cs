﻿namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    public sealed class AddNurseResponse
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DepartamentId { get; set; }

    }
}
