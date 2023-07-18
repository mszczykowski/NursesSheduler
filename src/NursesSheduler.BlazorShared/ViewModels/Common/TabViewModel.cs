using Microsoft.AspNetCore.Components;

namespace NursesScheduler.BlazorShared.ViewModels.Common
{
    public sealed class TabViewModel
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public ComponentBase PageContent { get; set; }
        public bool IsSelected { get; set; }
        public bool IsEnabled { get; set; }
    }
}
