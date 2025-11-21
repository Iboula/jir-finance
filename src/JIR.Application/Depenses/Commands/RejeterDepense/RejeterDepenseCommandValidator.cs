using FluentValidation;

namespace JIR.Application.Depenses.Commands.RejeterDepense;

public class RejeterDepenseCommandValidator : AbstractValidator<RejeterDepenseCommand>
{
    public RejeterDepenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID de la dépense est obligatoire.");

        RuleFor(x => x.MotifRejet)
            .NotEmpty().WithMessage("Le motif de rejet est obligatoire.")
            .MaximumLength(500).WithMessage("Le motif de rejet ne doit pas dépasser 500 caractères.");

        RuleFor(x => x.RejetePar)
            .NotEmpty().WithMessage("Le nom de la personne qui rejette est obligatoire.")
            .MaximumLength(100).WithMessage("Le nom ne doit pas dépasser 100 caractères.");
    }
}
