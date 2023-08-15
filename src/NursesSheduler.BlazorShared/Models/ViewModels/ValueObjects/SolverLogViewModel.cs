using NursesScheduler.BlazorShared.Extensions;
using NursesScheduler.BlazorShared.Models.Enums;

namespace NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects
{
    public sealed class SolverLogViewModel
    {
        public DateTime LogDate { get; set; }
        public SolverEvents SolverEvent { get; set; }
        public SolverAbortReasons? AbortReason { get; set; }
        public int CurrentSolverRun { get; set; }

        public override string ToString()
        {
            return $"{LogDate}: {SolverEvent.GetEnumDisplayName()}, próba: {CurrentSolverRun}";
        }
    }
}
