using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class SaveSystemUserCommandValidator : AbstractValidator<SaveSystemUserCommand>
    {
        public SaveSystemUserCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.PasswordHash)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
