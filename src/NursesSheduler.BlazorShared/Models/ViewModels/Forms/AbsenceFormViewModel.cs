using NursesScheduler.BlazorShared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Forms
{
    public sealed class AbsenceFormViewModel : IValidatableObject
    {
        public int NurseId { get; set; }
        public int AbsenceId { get; set; }

        private DateOnly _from;

        [Required(ErrorMessage = "Należy podać datę początkową")]
        public DateOnly From 
        {
            get => _from; 
            set
            {
                _from = value;
                if(_from > To)
                {
                    To = _from;
                }
            }
        }

        [Required(ErrorMessage = "Należy podać datę końcową")]
        public DateOnly To { get; set; }

        [Required(ErrorMessage = "Należy wybrać typ")]
        public AbsenceTypes Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To < From)
            {
                yield return new ValidationResult("Data końcowa nie może być wcześniejsza niż początkowa");
            }
        }
    }
}
