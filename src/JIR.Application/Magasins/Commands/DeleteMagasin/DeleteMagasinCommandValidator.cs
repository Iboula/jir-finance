using FluentValidation;

namespace JIR.Application.Magasins.Commands.DeleteMagasin;

public class DeleteMagasinCommandValidator : AbstractValidator<DeleteMagasinCommand>
{
    public DeleteMagasinCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");
    }
}
