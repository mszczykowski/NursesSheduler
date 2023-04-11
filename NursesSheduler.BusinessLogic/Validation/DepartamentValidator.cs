using FluentValidation;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class DepartamentValidator : AbstractValidator<Departament>
    {
        public DepartamentValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .MaximumLength(40);
        }
    }
}
