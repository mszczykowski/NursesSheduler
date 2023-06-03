using Microsoft.AspNetCore.Components;
using NursesScheduler.BlazorShared.Stores;

namespace NursesScheduler.BlazorShared.Abstracions
{
    public abstract class ComponenentUsingDepartament : ExceptionHandlingComponent
    {
        [Inject]
        protected CurrentDepartamentStore CurrentDepartamentStore { get; set; }
        protected override void OnInitialized()
        {
            if (CurrentDepartamentStore.CurrentDepartament == null)
                NavigationManager.NavigateTo("/departaments");
            
            base.OnInitialized();
        }
    }
}
