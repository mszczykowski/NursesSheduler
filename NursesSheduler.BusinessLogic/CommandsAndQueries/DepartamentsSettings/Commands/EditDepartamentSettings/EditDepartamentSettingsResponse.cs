﻿namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings
{
    public sealed class EditDepartamentSettingsResponse
    {
        public int SettingsId { get; set; }

        public TimeSpan WorkingTime { get; set; }

        public TimeSpan MaximalWeekWorkingTime { get; set; }

        public TimeSpan MinmalShiftBreak { get; set; }

        public int FirstQuarterStart { get; set; }

        public TimeOnly FirstShiftStartTime { get; set; }

        public int TargetNumberOfNursesOnShift { get; set; }

        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }

        public int DefaultGeneratorRetryValue { get; set; }
    }
}
