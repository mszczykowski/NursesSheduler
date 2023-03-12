using Microsoft.AspNetCore.Components;

namespace NursesScheduler.BlazorShared.Shared
{
    public abstract class ModalBase : ComponentBase
    {
        protected bool _isVisible;

        protected void ChangeVisibility()
        {
            _isVisible = !_isVisible;
            StateHasChanged();
        }
    }
}
