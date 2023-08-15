using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.Enums
{
    public enum AbsenceTypes
    {
        [Display(Name = "Urlop wypoczynkowy")]
        PersonalTimeOff,
        [Display(Name = "Urlop na żądanie")]
        LeaveOnRequest,
        [Display(Name = "Urlop rodzicielski")]
        ParentalLeave,
        [Display(Name = "Zwolnienie lekarskie")]
        SickLeave,
        [Display(Name = "Inne")]
        Other
    }
}
