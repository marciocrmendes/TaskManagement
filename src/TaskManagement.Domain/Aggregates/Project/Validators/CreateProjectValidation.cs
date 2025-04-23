using FluentValidation;
using TaskManagement.Domain.Aggregates.Project.Commands;

namespace TaskManagement.Domain.Aggregates.Project.Validators
{
    public class CreateProjectValidation : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("O nome precisa ser preenchido.")
                .MaximumLength(100)
                    .WithMessage("O nome tem mais de 100 caracteres.");

            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(3000)
                        .WithMessage("A descrição tem mais de 3000 caracteres.");
            });
        }
    }
}
