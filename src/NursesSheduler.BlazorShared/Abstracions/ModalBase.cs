using Microsoft.AspNetCore.Components;

namespace NursesScheduler.BlazorShared.Abstracions
{
    public abstract class ModalBase : ComponentBase
    {
        protected bool _isVisible;

        public virtual void ShowModal()
        {
            ChangeVisibility();
        }

        protected void ChangeVisibility()
        {
            _isVisible = !_isVisible;
            StateHasChanged();
        }
    }
}
