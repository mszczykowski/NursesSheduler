using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NursesScheduler.BlazorShared.Extensions
{
    internal static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }
    }
}
