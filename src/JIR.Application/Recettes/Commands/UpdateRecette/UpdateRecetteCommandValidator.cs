using FluentValidation;

namespace JIR.Application.Recettes.Commands.UpdateRecette;

public class UpdateRecetteCommandValidator : AbstractValidator<UpdateRecetteCommand>
{
    public UpdateRecetteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant de la recette est requis.");

        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("L'identifiant de la section est requis.");

        RuleFor(x => x.Libelle)
            .NotEmpty().WithMessage("Le libellé est requis.")
            .MaximumLength(200).WithMessage("Le libellé ne peut pas dépasser 200 caractères.");

        RuleFor(x => x.Montant)
            .GreaterThan(0).WithMessage("Le montant doit être supérieur à 0.");

        RuleFor(x => x.DateRecette)
            .NotEmpty().WithMessage("La date de recette est requise.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La date de recette ne peut pas être dans le futur.");

        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("La source est requise.")
            .MaximumLength(200).WithMessage("La source ne peut pas dépasser 200 caractères.");

        RuleFor(x => x.Categorie)
            .NotEmpty().WithMessage("La catégorie est requise.")
            .MaximumLength(100).WithMessage("La catégorie ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.RecuNumero)
            .MaximumLength(50).WithMessage("Le numéro de reçu ne peut pas dépasser 50 caractères.")
            .When(x => !string.IsNullOrEmpty(x.RecuNumero));

        RuleFor(x => x.ModeEncaissement)
            .NotEmpty().WithMessage("Le mode d'encaissement est requis.")
            .MaximumLength(50).WithMessage("Le mode d'encaissement ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.NumeroDocument)
            .MaximumLength(100).WithMessage("Le numéro de document ne peut pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrEmpty(x.NumeroDocument));

        RuleFor(x => x.PieceJustificative)
            .MaximumLength(500).WithMessage("La pièce justificative ne peut pas dépasser 500 caractères.")
            .When(x => !string.IsNullOrEmpty(x.PieceJustificative));

        RuleFor(x => x.Observations)
            .MaximumLength(1000).WithMessage("Les observations ne peuvent pas dépasser 1000 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
