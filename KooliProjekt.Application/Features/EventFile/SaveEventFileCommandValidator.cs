using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class SaveEventFileCommandValidator : AbstractValidator<SaveEventFileCommand>
    {
        public SaveEventFileCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0);

            RuleFor(x => x.FilePath)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
