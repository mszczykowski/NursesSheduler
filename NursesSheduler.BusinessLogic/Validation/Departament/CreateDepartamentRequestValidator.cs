using FluentValidation;
using NursesScheduler.BusinessLogic.Departaments.Commands.CreateDepartament;

namespace NursesScheduler.BusinessLogic.Validation.Departament
{
    internal class CreateDepartamentRequestValidator : AbstractValidator<CreateDepartamentRequest>
    {
        public CreateDepartamentRequestValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
