﻿namespace NursesScheduler.Domain.ValueObjects
{
    public sealed record DayNumbered : Day
    {
        public int DayInQuarter { get; set; }
        public int WeekInQuarter => (int)Math.Ceiling((decimal)DayInQuarter / 7);
    }
}
