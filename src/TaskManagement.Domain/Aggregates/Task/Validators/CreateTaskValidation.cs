using FluentValidation;
using TaskManagement.Domain.Aggregates.Task.Commands;

namespace TaskManagement.Domain.Aggregates.Task.Validators
{
    public class CreateTaskValidation : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskValidation()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("O projeto precisa ser informado.");

            RuleFor(x => x.Title)
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
