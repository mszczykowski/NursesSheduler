using NursesScheduler.BlazorShared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class AbsenceFormViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Należy podać datę początkową")]
        public DateOnly From { get; set; }
        [Required(ErrorMessage = "Należy podać datę końcową")]
        public DateOnly To { get; set; }
        [Required(ErrorMessage = "Należy wybrać typ")]
        public AbsenceTypes Type { get; set; }
        public int AbsencesSummaryId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To < From)
            {
                yield return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa");
            }
        }
    }
}
