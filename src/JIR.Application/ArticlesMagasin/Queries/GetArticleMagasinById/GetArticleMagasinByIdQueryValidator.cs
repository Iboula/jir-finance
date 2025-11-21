using FluentValidation;

namespace JIR.Application.ArticlesMagasin.Queries.GetArticleMagasinById;

public class GetArticleMagasinByIdQueryValidator : AbstractValidator<GetArticleMagasinByIdQuery>
{
    public GetArticleMagasinByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");
    }
}
