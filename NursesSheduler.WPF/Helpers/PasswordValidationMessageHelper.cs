using NursesScheduler.WPF.Models.Enums;
using System.Windows;

namespace NursesScheduler.WPF.Helpers
{
    internal static class PasswordValidationMessageHelper
    {
        public static string GetValidationMessage(PasswordValidationResult validationResult)
        {
            switch(validationResult)
            {
                case PasswordValidationResult.Empty:
                    return (string)Application.Current.FindResource("localizedMessage");
                case PasswordValidationResult.ToShort:
                    return (string)Application.Current.FindResource("localizedMessage");
                case PasswordValidationResult.NoNumber:
                    return "";
                case PasswordValidationResult.NoLowerOrUpper:
                    return "";
                default:
                    return "";
            }
        }
    }
}
