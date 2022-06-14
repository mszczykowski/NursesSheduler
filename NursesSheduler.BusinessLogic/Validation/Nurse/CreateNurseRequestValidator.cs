using FluentValidation;
using NursesScheduler.BusinessLogic.Nurses.Commands.CreateNurse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Validation.Nurse
{
    internal class CreateNurseRequestValidator : AbstractValidator<CreateNurseRequest>
    {
        public CreateNurseRequestValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(n => n.Surname)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
