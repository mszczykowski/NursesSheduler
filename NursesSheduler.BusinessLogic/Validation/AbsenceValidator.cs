using FluentValidation;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class AbsenceValidator : AbstractValidator<Absence>
    {
        public AbsenceValidator()
        {
            RuleFor(a => a.From)
                .NotEmpty();
            RuleFor(a => a.From.Year)
                .Equal(a => a.To.Year);
            RuleFor(a => a.To)
                .NotEmpty()
                .GreaterThanOrEqualTo(a => a.From);
            RuleFor(a => a.Type)
                .IsInEnum();
        }
    }
}
