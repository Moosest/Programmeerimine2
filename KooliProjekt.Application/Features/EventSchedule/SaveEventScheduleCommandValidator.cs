using System;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class SaveEventScheduleCommandValidator : AbstractValidator<SaveEventScheduleCommand>
    {
        public SaveEventScheduleCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0);

            RuleFor(x => x.StartTime)
                .NotEqual(DateTime.MinValue);

            RuleFor(x => x.FilePath)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
