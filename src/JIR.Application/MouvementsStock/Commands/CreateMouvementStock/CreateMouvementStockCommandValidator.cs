using FluentValidation;

namespace JIR.Application.MouvementsStock.Commands.CreateMouvementStock;

public class CreateMouvementStockCommandValidator : AbstractValidator<CreateMouvementStockCommand>
{
    public CreateMouvementStockCommandValidator()
    {
        RuleFor(x => x.ArticleId)
            .NotEmpty().WithMessage("L'identifiant de l'article est requis.");

        RuleFor(x => x.TypeMouvement)
            .IsInEnum().WithMessage("Le type de mouvement est invalide.");

        RuleFor(x => x.Quantite)
            .GreaterThan(0).WithMessage("La quantité doit être supérieure à zéro.");

        RuleFor(x => x.DateMouvement)
            .NotEmpty().WithMessage("La date du mouvement est requise.");

        RuleFor(x => x.NumeroDocument)
            .MaximumLength(100).WithMessage("Le numéro de document ne peut pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrEmpty(x.NumeroDocument));

        RuleFor(x => x.Motif)
            .MaximumLength(500).WithMessage("Le motif ne peut pas dépasser 500 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Motif));
    }
}
