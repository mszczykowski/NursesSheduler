using NursesScheduler.WPF.Models.Enums;
using System.Windows;

namespace NursesScheduler.WPF.Helpers
{
    internal static class PasswordValidationMessageHelper
    {
        public static string GetWrongPasswordMessage()
        {
            return (string)Application.Current.FindResource("wrong_passwd");
        }
        public static string GetNotMatchingPasswordMessage()
        {
            return (string)Application.Current.FindResource("passwd_doesnt_match");
        }
        public static string GetValidationMessage(PasswordValidationResult validationResult)
        {
            switch(validationResult)
            {
                case PasswordValidationResult.Empty:
                    return (string)Application.Current.FindResource("passwd_empty");
                case PasswordValidationResult.ToShort:
                    return (string)Application.Current.FindResource("passwd_min_lenght");
                case PasswordValidationResult.NoNumber:
                    return (string)Application.Current.FindResource("passwd_needs_number");
                case PasswordValidationResult.NoLowerOrUpper:
                    return (string)Application.Current.FindResource("passwd_lower_upper");
                default:
                    return "";
            }
        }
    }
}
