using FluentValidation;

namespace JIR.Application.Cotisations.Commands.UpdateCotisation;

public class UpdateCotisationCommandValidator : AbstractValidator<UpdateCotisationCommand>
{
    public UpdateCotisationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID est obligatoire.");

        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("La section est obligatoire.");

        RuleFor(x => x.MembreNom)
            .NotEmpty().WithMessage("Le nom du membre est obligatoire.")
            .MaximumLength(100).WithMessage("Le nom ne doit pas dépasser 100 caractères.");

        RuleFor(x => x.MembreMatricule)
            .NotEmpty().WithMessage("Le matricule du membre est obligatoire.")
            .MaximumLength(50).WithMessage("Le matricule ne doit pas dépasser 50 caractères.");

        RuleFor(x => x.Montant)
            .GreaterThan(0).WithMessage("Le montant doit être supérieur à 0.");

        RuleFor(x => x.Periode)
            .NotEmpty().WithMessage("La période est obligatoire.")
            .Matches(@"^\d{4}-(0[1-9]|1[0-2])$")
            .WithMessage("La période doit être au format YYYY-MM (exemple: 2025-01).");

        RuleFor(x => x.DatePaiement)
            .NotEmpty().WithMessage("La date de paiement est obligatoire.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La date de paiement ne peut pas être dans le futur.");

        RuleFor(x => x.ModePaiement)
            .MaximumLength(50).WithMessage("Le mode de paiement ne doit pas dépasser 50 caractères.")
            .When(x => !string.IsNullOrEmpty(x.ModePaiement));

        RuleFor(x => x.RecuNumero)
            .MaximumLength(50).WithMessage("Le numéro de reçu ne doit pas dépasser 50 caractères.")
            .When(x => !string.IsNullOrEmpty(x.RecuNumero));

        RuleFor(x => x.Observations)
            .MaximumLength(500).WithMessage("Les observations ne doivent pas dépasser 500 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
