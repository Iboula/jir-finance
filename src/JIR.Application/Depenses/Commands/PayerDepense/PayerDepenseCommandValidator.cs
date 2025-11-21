using FluentValidation;

namespace JIR.Application.Depenses.Commands.PayerDepense;

public class PayerDepenseCommandValidator : AbstractValidator<PayerDepenseCommand>
{
    public PayerDepenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'ID de la dépense est obligatoire.");

        RuleFor(x => x.DatePaiement)
            .NotEmpty().WithMessage("La date de paiement est obligatoire.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La date de paiement ne peut pas être dans le futur.");

        RuleFor(x => x.ModePaiement)
            .NotEmpty().WithMessage("Le mode de paiement est obligatoire.")
            .MaximumLength(50).WithMessage("Le mode de paiement ne doit pas dépasser 50 caractères.");

        RuleFor(x => x.ReferenceFacture)
            .MaximumLength(100).WithMessage("La référence facture ne doit pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrEmpty(x.ReferenceFacture));

        RuleFor(x => x.Observations)
            .MaximumLength(1000).WithMessage("Les observations ne doivent pas dépasser 1000 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Observations));
    }
}
