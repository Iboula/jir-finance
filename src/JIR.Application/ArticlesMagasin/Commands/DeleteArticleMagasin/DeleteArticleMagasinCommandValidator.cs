using FluentValidation;

namespace JIR.Application.ArticlesMagasin.Commands.DeleteArticleMagasin;

public class DeleteArticleMagasinCommandValidator : AbstractValidator<DeleteArticleMagasinCommand>
{
    public DeleteArticleMagasinCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");
    }
}
