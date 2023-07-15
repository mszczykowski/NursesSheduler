using Microsoft.AspNetCore.Components.Forms;

namespace NursesScheduler.BlazorShared.Helpers
{
    internal class CustomFieldClassHelper : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext,
        in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return isValid ? String.Empty : "is-danger";
        }
    }
}
