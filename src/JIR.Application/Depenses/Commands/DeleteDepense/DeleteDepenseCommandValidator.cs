using FluentValidation;

namespace JIR.Application.Depenses.Commands.DeleteDepense;

public class DeleteDepenseCommandValidator : AbstractValidator<DeleteDepenseCommand>
{
    public DeleteDepenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant de la dépense est requis.");
    }
}
