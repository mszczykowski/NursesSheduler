﻿namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Managers
{
    internal interface IShiftCapacityManager
    {
        public bool IsSwappingRegularForMorningSuggested { get; }
        public bool IsSwappingRequired { get; }
        public int TargetMinimalNumberOfNursesOnShift { get; }
        public int ActualMinimalNumberOfNursesOnShift { get; }
    }
}