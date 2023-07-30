using NursesScheduler.BlazorShared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class NurseViewModel
    {
        public int NurseId { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane")]
        [StringLength(20, ErrorMessage = "Imię jest zbyt długie (maksymalnie 20 znaków).")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(30, ErrorMessage = "Nzwisko jest zbyt długie (maksymalnie 30 znaków).")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Należy wybrać opcję")]
        [Range(20, 26, ErrorMessage = "Należy wybrać prawidłową wartość")]
        public int PTOentitlement { get; set; }

        public int DepartamentId { get; set; }
        public NurseTeams NurseTeam { get; set; }

        public TimeSpan NightHoursBalance { get; set; }
        public TimeSpan HolidayHoursBalance { get; set; }

        public bool IsDeleted { get; set; }

        public NurseViewModel()
        {
            PTOentitlement = 26;
        }
        public override string ToString()
        {
            return $"{Surname} {Name}";
        }
    }
}
