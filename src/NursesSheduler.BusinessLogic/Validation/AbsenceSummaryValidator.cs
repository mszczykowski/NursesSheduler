using FluentValidation;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class AbsenceSummaryValidator : AbstractValidator<AbsencesSummary>
    {
        public AbsenceSummaryValidator()
        {
            RuleFor(s => s.PTOTimeLeft)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero);
            RuleFor(s => s.PTOTimeLeftFromPreviousYear)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero);
        }
    }
}
