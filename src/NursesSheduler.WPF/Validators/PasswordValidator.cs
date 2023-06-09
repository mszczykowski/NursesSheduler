using NursesScheduler.WPF.Models.Enums;
using System.Linq;

namespace NursesScheduler.WPF.Validators
{
    internal static class PasswordValidator
    {
        public static readonly int MIN_PASSWORD_LENGHT = 8;

        public static PasswordValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return PasswordValidationResult.ToShort;
            }
            if (password.Length < MIN_PASSWORD_LENGHT)
            {
                return PasswordValidationResult.ToShort;
            }
            if (!password.Any(char.IsDigit))
            {
                return PasswordValidationResult.NoNumber;
            }
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower))
            {
                return PasswordValidationResult.NoLowerOrUpper;
            }

            return PasswordValidationResult.Valid;
        }
    }
}
