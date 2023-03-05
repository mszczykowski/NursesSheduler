using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
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
        public int DepartamentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}
