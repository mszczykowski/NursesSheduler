using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Services
{
    public sealed class WorkTimeService
    {
        TimeSpan GetHoursToNextShift(ShiftTypes workDayShift)
        {
            switch (workDayShift)
            {
                case ShiftTypes.None:
                    return TimeSpan.FromDays(1);
                case ShiftTypes.Day:
                    return TimeSpan.Zero;
                case ShiftTypes.Night:
                    return GeneralConstants.RegularShiftLenght;
                case ShiftTypes.Morning:
                    return TimeSpan.Zero;
                default:
                    return TimeSpan.Zero;
            }
        }

        TimeSpan GetHoursFromPreviousShift(ShiftTypes workDayShift, TimeSpan? morningShiftLength)
        {
            switch (workDayShift)
            {
                case ShiftTypes.None:
                    return TimeSpan.FromDays(1);
                case ShiftTypes.Day:
                    return GeneralConstants.RegularShiftLenght;
                case ShiftTypes.Night:
                    return TimeSpan.Zero;
                case ShiftTypes.Morning:
                    return 2 * GeneralConstants.RegularShiftLenght - (TimeSpan)morningShiftLength;
                default:
                    return TimeSpan.Zero;
            }
        }
    }
}
