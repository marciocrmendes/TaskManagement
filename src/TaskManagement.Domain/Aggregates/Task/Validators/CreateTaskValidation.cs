using FluentValidation;
using TaskManagement.Domain.Aggregates.Task.Commands;

namespace TaskManagement.Domain.Aggregates.Task.Validators
{
    public class CreateTaskValidation : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("O título precisa ser preenchido.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A descrição precisa ser preenchida.");

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("O projeto precisa ser informado.");
        }
    }
}
