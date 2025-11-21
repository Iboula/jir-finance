using FluentValidation;

namespace JIR.Application.Depenses.Commands.CreateDepense;

public class CreateDepenseCommandValidator : AbstractValidator<CreateDepenseCommand>
{
    public CreateDepenseCommandValidator()
    {
        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("La section est obligatoire.");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("Le numéro de dépense est obligatoire.")
            .MaximumLength(50).WithMessage("Le numéro ne doit pas dépasser 50 caractères.");

        RuleFor(x => x.DateDepense)
            .NotEmpty().WithMessage("La date de dépense est obligatoire.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La date de dépense ne peut pas être dans le futur.");

        RuleFor(x => x.Categorie)
            .NotEmpty().WithMessage("La catégorie est obligatoire.")
            .MaximumLength(100).WithMessage("La catégorie ne doit pas dépasser 100 caractères.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La description est obligatoire.")
            .MaximumLength(500).WithMessage("La description ne doit pas dépasser 500 caractères.");

        RuleFor(x => x.Montant)
            .GreaterThan(0).WithMessage("Le montant doit être supérieur à 0.");

        RuleFor(x => x.Beneficiaire)
            .NotEmpty().WithMessage("Le bénéficiaire est obligatoire.")
            .MaximumLength(200).WithMessage("Le bénéficiaire ne doit pas dépasser 200 caractères.");

        RuleFor(x => x.ModePaiement)
            .MaximumLength(50).WithMessage("Le mode de paiement ne doit pas dépasser 50 caractères.")
            .When(x => !string.IsNullOrEmpty(x.ModePaiement));

        RuleFor(x => x.ReferenceFacture)
            .MaximumLength(100).WithMessage("La référence facture ne doit pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrEmpty(x.ReferenceFacture));

        RuleFor(x => x.Observations)
            .MaximumLength(1000).WithMessage("Les observations ne doivent pas dépasser 1000 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
