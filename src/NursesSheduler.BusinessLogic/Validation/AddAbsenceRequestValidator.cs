﻿using FluentValidation;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;

namespace NursesScheduler.BusinessLogic.Validation
{
    internal sealed class AddAbsenceRequestValidator : AbstractValidator<AddAbsenceRequest>
    {
        public AddAbsenceRequestValidator()
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
