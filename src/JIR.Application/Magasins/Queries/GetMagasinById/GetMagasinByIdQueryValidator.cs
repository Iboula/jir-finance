using FluentValidation;

namespace JIR.Application.Magasins.Queries.GetMagasinById;

public class GetMagasinByIdQueryValidator : AbstractValidator<GetMagasinByIdQuery>
{
    public GetMagasinByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");
    }
}
