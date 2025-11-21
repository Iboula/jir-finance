using FluentValidation;

namespace JIR.Application.Sections.Commands.UpdateSection;

public class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("L'ID est requis.");

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Le code est requis.")
            .MaximumLength(20).WithMessage("Le code ne doit pas dépasser 20 caractères.");

        RuleFor(v => v.Nom)
            .NotEmpty().WithMessage("Le nom est requis.")
            .MaximumLength(200).WithMessage("Le nom ne doit pas dépasser 200 caractères.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("La description ne doit pas dépasser 500 caractères.");

        RuleFor(v => v.BudgetAnnuel)
            .GreaterThanOrEqualTo(0).WithMessage("Le budget annuel doit être positif.");
    }
}
