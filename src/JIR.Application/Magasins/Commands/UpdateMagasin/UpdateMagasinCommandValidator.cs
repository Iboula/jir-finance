using FluentValidation;

namespace JIR.Application.Magasins.Commands.UpdateMagasin;

public class UpdateMagasinCommandValidator : AbstractValidator<UpdateMagasinCommand>
{
    public UpdateMagasinCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Le code est requis.")
            .MaximumLength(50).WithMessage("Le code ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Nom)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MaximumLength(200).WithMessage("Le nom ne peut pas dépasser 200 caractères.");

        RuleFor(x => x.Localisation)
            .MaximumLength(500).WithMessage("La localisation ne peut pas dépasser 500 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Localisation));
    }
}
