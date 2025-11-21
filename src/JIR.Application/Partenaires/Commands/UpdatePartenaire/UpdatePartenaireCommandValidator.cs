using FluentValidation;

namespace JIR.Application.Partenaires.Commands.UpdatePartenaire;

public class UpdatePartenaireCommandValidator : AbstractValidator<UpdatePartenaireCommand>
{
    public UpdatePartenaireCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Le code est requis.")
            .MaximumLength(50).WithMessage("Le code ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Nom)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MaximumLength(200).WithMessage("Le nom ne peut pas dépasser 200 caractères.");

        RuleFor(x => x.TypePartenaire)
            .IsInEnum().WithMessage("Le type de partenaire est invalide.");

        RuleFor(x => x.Adresse)
            .MaximumLength(500).WithMessage("L'adresse ne peut pas dépasser 500 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Adresse));

        RuleFor(x => x.Telephone)
            .MaximumLength(20).WithMessage("Le téléphone ne peut pas dépasser 20 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Telephone));

        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("L'email ne peut pas dépasser 100 caractères.")
            .EmailAddress().WithMessage("L'email n'est pas valide.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.ContactNom)
            .MaximumLength(100).WithMessage("Le nom du contact ne peut pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrEmpty(x.ContactNom));

        RuleFor(x => x.Observations)
            .MaximumLength(1000).WithMessage("Les observations ne peuvent pas dépasser 1000 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
