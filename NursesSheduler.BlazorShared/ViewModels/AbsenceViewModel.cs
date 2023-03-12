using NursesScheduler.BlazorShared.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class AbsenceViewModel : IValidatableObject
    {
        public int AbsenceId { get; set; }
        [Required(ErrorMessage = "Należy podać datę początkową")]
        public DateOnly From { get; set; }
        [Required(ErrorMessage = "Należy podać datę końcową")]
        public DateOnly To { get; set; }
        [Required(ErrorMessage = "Należy wybrać typ")]
        public AbsenceTypes Type { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public int YearlyAbsencesSummaryId { get; set; }

        public int Lenght => To.DayNumber - From.DayNumber;

        public override string ToString()
        {
            return $"{From} - {To}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(To < From)
            {
                yield return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa");
            }
        }
    }
}
