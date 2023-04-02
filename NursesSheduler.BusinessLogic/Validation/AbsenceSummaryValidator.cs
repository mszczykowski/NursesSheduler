using FluentValidation;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal class AbsenceSummaryValidator : AbstractValidator<AbsencesSummary>
    {
        public AbsenceSummaryValidator()
        {
            RuleFor(s => s.PTOTime)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero);
            RuleFor(s => s.PTOTimeUsed)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero);
            RuleFor(s => s.PTOTimeLeftFromPreviousYear)
                .NotEmpty()
                .GreaterThanOrEqualTo(TimeSpan.Zero);
        }
    }
}
