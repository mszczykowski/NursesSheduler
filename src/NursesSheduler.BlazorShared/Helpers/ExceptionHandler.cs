using Microsoft.AspNetCore.Components;
using NursesScheduler.BlazorShared.Abstracions;
using NursesScheduler.BlazorShared.Stores;

namespace NursesScheduler.BlazorShared.Helpers
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        private readonly ExceptionStore _exceptionStore;
        private readonly NavigationManager _navigationManager;

        public ExceptionHandler(ExceptionStore exceptionStore, NavigationManager navigationManager)
        {
            _exceptionStore = exceptionStore;
            _navigationManager = navigationManager;
        }

        public void HandleException(Exception e)
        {
            _exceptionStore.Exception = e;
            _navigationManager.NavigateTo("/error");
        }
    }
}
