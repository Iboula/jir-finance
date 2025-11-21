using FluentValidation;

namespace JIR.Application.Partenaires.Queries.GetPartenaireById;

public class GetPartenaireByIdQueryValidator : AbstractValidator<GetPartenaireByIdQuery>
{
    public GetPartenaireByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");
    }
}
