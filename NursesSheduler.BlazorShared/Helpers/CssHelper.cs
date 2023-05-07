namespace NursesScheduler.BlazorShared.Helpers
{
    internal static class CssHelper
    {
        public static string SetIsLoading(bool isLoading)
        {
            if (isLoading) return "is-loading";
            return String.Empty;
        }

        public static string SetIsActive(bool isActive)
        {
            if (isActive) return "is-active";
            return String.Empty;
        }

        public static string SetVisibility(bool isVisible)
        {
            if (!isVisible) return "display: none";
            return String.Empty;
        }
    }
}