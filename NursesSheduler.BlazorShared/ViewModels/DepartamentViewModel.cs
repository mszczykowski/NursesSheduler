using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class DepartamentViewModel
    {
        public int DepartamentId { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [StringLength(40, ErrorMessage = "Nazwa zbyt długa (maksymalnie 40 znaków).")]
        public string Name { get; set; }

        public int CreationYear { get; set; }
    }
}
