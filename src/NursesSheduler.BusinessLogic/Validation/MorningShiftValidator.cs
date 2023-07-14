using FluentValidation;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class MorningShiftValidator : AbstractValidator<MorningShift>
    {
        public MorningShiftValidator()
        {
            RuleFor(m => m.Index)
                .IsInEnum();
            RuleFor(m => m.ShiftLength)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero)
                .LessThanOrEqualTo(TimeSpan.FromHours(12));
        }
    }
}
