using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels.Enums
{
    public enum AbsenceVeryficationResult
    {
        Valid,
        [Display(Name = "Rok nie może być różny od aktywnego roku podsumowania")]
        InvalidYear,
        [Display(Name = "Istnieje już urlop który zawiera się w wybranym zakresie. Urlop nie został zapisany.")]
        AbsenceAlreadyExists,
    }
}
