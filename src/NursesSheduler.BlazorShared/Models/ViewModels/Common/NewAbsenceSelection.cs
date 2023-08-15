using NursesScheduler.BlazorShared.Models.ViewModels.Entities;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Common
{
    public sealed class NewAbsenceSelection
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public NurseViewModel Nurse { get; set; }
    }
}
