using Microsoft.AspNetCore.Components;
using NursesScheduler.BlazorShared.Stores;

namespace NursesScheduler.BlazorShared.Pages
{
    public abstract class PageBase : ComponentBase
    {
        [Inject]
        protected ExceptionStore ExceptionStore { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected void HandleException(Exception e)
        {
            ExceptionStore.Exception = e;
            NavigationManager.NavigateTo("/error");
        }
    }
}