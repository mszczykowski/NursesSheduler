using NursesScheduler.BlazorShared.ViewModels;

namespace NursesScheduler.BlazorShared.Stores
{
    public sealed class CurrentDepartamentStore
    {
        private DepartamentViewModel _currentDepartament;
        public event Action OnChange;
        public DepartamentViewModel CurrentDepartament
        {
            get => _currentDepartament;
            set
            {
                _currentDepartament = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
