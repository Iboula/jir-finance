using FluentValidation;

namespace JIR.Application.Recettes.Commands.DeleteRecette;

public class DeleteRecetteCommandValidator : AbstractValidator<DeleteRecetteCommand>
{
    public DeleteRecetteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant de la recette est requis.");
    }
}
