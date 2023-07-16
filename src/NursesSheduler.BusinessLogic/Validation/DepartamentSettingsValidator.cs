using FluentValidation;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class DepartamentSettingsValidator : AbstractValidator<DepartamentSettings>
    {
        public DepartamentSettingsValidator()
        {
            RuleFor(s => s.WorkDayLength)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.FromHours(1))
                .LessThanOrEqualTo(TimeSpan.FromHours(12));

            RuleFor(s => s.MaximalWeekWorkDayLength)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.FromHours(7))
                .LessThanOrEqualTo(TimeSpan.FromHours(84));

            RuleFor(s => s.MinmalShiftBreak)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero)
                .LessThanOrEqualTo(TimeSpan.FromHours(100));

            RuleFor(s => s.FirstQuarterStart)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(12);

            RuleFor(s => s.TargetMinNumberOfNursesOnShift)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(100);

            RuleFor(s => s.TargetMinimalMorningShiftLenght)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.FromHours(1))
                .LessThanOrEqualTo(TimeSpan.FromHours(12));

            RuleFor(s => s.DefaultGeneratorRetryValue)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(10);
        }
    }
}
