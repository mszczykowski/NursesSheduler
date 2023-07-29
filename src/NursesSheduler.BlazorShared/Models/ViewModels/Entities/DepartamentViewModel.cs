using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class DepartamentViewModel
    {
        public int DepartamentId { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [StringLength(40, ErrorMessage = "Nazwa zbyt długa (maksymalnie 40 znaków).")]
        public string Name { get; set; }
        public int CreationYear { get; set; }
        [Required(ErrorMessage = "Należy wybrać pierwszy miesiąc pierwszego kwartału")]
        [Range(1, 12, ErrorMessage = "Wartość musi być miesiącem")]
        public int FirstQuarterStart { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }

        public bool UseTeamsSetting { get; set; }

        public DepartamentViewModel()
        {
            FirstQuarterStart = 1;
        }
    }
}
