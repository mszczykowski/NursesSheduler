namespace NursesScheduler.BlazorShared.Helpers
{
    internal static class SubmitButtonCssHelper
    {
        public static string GetSubmitButtonCssClass(bool isLoading)
        {
            if (isLoading) return "button is-primary is-loading";
            else return "button is-primary";
        }
    }
}
