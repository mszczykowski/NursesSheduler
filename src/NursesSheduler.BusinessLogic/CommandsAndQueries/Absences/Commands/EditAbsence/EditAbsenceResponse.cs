﻿using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence
{
    public sealed class EditAbsenceResponse
    {
        public AbsenceResponse? Absence { get; set; }
        public AbsenceVeryficationResult VeryficationResult { get; set; }

        public sealed class AbsenceResponse
        {
            public int AbsenceId { get; set; }
            public int Month { get; set; }
            public ICollection<int> Days { get; set; }
            public TimeSpan WorkTimeToAssign { get; set; }
            public TimeSpan AssignedWorkingHours { get; set; }
            public AbsenceTypes Type { get; set; }
            public int AbsencesSummaryId { get; set; }
        }
    }
}
