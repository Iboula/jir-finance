using FluentValidation;

namespace JIR.Application.Depenses.Commands.ValiderDepense;

public class ValiderDepenseCommandValidator : AbstractValidator<ValiderDepenseCommand>
{
    public ValiderDepenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID de la dépense est obligatoire.");

        RuleFor(x => x.ValidePar)
            .NotEmpty().WithMessage("Le nom du validateur est obligatoire.")
            .MaximumLength(100).WithMessage("Le nom du validateur ne doit pas dépasser 100 caractères.");

        RuleFor(x => x.Observations)
            .MaximumLength(1000).WithMessage("Les observations ne doivent pas dépasser 1000 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
