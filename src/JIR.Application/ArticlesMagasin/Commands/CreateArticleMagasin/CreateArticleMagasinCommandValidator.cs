using FluentValidation;

namespace JIR.Application.ArticlesMagasin.Commands.CreateArticleMagasin;

public class CreateArticleMagasinCommandValidator : AbstractValidator<CreateArticleMagasinCommand>
{
    public CreateArticleMagasinCommandValidator()
    {
        RuleFor(x => x.MagasinId)
            .NotEmpty().WithMessage("L'identifiant du magasin est requis.");

        RuleFor(x => x.CodeArticle)
            .NotEmpty().WithMessage("Le code article est requis.")
            .MaximumLength(50).WithMessage("Le code article ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Designation)
            .NotEmpty().WithMessage("La désignation est requise.")
            .MaximumLength(500).WithMessage("La désignation ne peut pas dépasser 500 caractères.");

        RuleFor(x => x.Unite)
            .NotEmpty().WithMessage("L'unité est requise.")
            .MaximumLength(50).WithMessage("L'unité ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.SeuilAlerte)
            .GreaterThanOrEqualTo(0).WithMessage("Le seuil d'alerte doit être positif ou nul.");

        RuleFor(x => x.PrixUnitaire)
            .GreaterThanOrEqualTo(0).WithMessage("Le prix unitaire doit être positif ou nul.");

        RuleFor(x => x.Observations)
            .MaximumLength(1000).WithMessage("Les observations ne peuvent pas dépasser 1000 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
