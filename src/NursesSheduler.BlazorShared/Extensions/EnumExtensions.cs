using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NursesScheduler.BlazorShared.Extensions
{
    internal static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum enumType)
        {
            if(enumType is null)
            {
                throw new InvalidOperationException("Cannot display enum name for null");
            }

            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }
    }
}
