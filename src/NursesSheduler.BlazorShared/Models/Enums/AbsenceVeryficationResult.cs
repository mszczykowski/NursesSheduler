using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.Enums
{
    public enum AbsenceVeryficationResult
    {
        Valid,
        [Display(Name = "Nie można dodac urlopu do zamkniętego już miesiąca")]
        ClosedMonth,
        [Display(Name = "Istnieje już urlop który zawiera się w wybranym zakresie. Urlop nie został zapisany.")]
        AbsenceAlreadyExists,
    }
}
