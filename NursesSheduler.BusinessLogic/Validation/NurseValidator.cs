using FluentValidation;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal class NurseValidator : AbstractValidator<Nurse>
    {
        public NurseValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(n => n.Surname)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(n => n.PTOentitlement)
                .NotEmpty();
        }
    }
}
