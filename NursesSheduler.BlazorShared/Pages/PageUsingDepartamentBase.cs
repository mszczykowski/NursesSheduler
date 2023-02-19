using Microsoft.AspNetCore.Components;
using NursesScheduler.BlazorShared.Stores;

namespace NursesScheduler.BlazorShared.Pages
{
    public abstract class PageUsingDepartamentBase : PageBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected CurrentDepartamentStore CurrentDepartamentStore { get; set; }
        protected override Task OnInitializedAsync()
        {
            if (CurrentDepartamentStore.CurrentDepartament == null)
                NavigationManager.NavigateTo("/departaments");
            return base.OnInitializedAsync();
        }
    }
}
