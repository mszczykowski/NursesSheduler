﻿namespace SolverService.Domain.Models.Calendar
{
    public sealed class Quarter
    {
        public Month[] Months { get; set; }
        public List<TimeSpan> SurplusShifts { get; set; }
    }
}
