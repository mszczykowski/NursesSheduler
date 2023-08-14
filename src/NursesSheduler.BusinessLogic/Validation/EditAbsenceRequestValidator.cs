
using FluentValidation;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class EditAbsenceRequestValidator : AbstractValidator<EditAbsenceRequest>
    {
        public EditAbsenceRequestValidator()
        {
            RuleFor(a => a.From)
                .NotEmpty();
            RuleFor(a => a.From.Year)
                .Equal(a => a.To.Year);
            RuleFor(a => a.To)
                .NotEmpty()
                .GreaterThanOrEqualTo(a => a.From);
            RuleFor(a => a.To.Month)
                .Equal(a => a.From.Month);
            RuleFor(a => a.Type)
                .IsInEnum();
        }
    }
}
