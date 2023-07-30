using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.Enums
{
    public enum SolverAbortReasons
    {
        [Display(Name = "zbyt mała liczba pielęgniarek")]
        NotEnoughNurses,
    }
}
