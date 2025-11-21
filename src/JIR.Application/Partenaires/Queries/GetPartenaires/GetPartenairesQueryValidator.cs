using FluentValidation;

namespace JIR.Application.Partenaires.Queries.GetPartenaires;

public class GetPartenairesQueryValidator : AbstractValidator<GetPartenairesQuery>
{
    public GetPartenairesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Le numéro de page doit être supérieur ou égal à 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("La taille de page doit être supérieure ou égale à 1.")
            .LessThanOrEqualTo(100).WithMessage("La taille de page ne peut pas dépasser 100.");

        RuleFor(x => x.TypePartenaire)
            .IsInEnum().WithMessage("Le type de partenaire est invalide.")
            .When(x => x.TypePartenaire.HasValue);

        RuleFor(x => x.Search)
            .MaximumLength(200).WithMessage("Le terme de recherche ne peut pas dépasser 200 caractères.")
            .When(x => !string.IsNullOrEmpty(x.Search));
    }
}
