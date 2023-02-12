using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class DepartamentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
