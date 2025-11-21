using FluentValidation;

namespace JIR.Application.Partenaires.Commands.DeletePartenaire;

public class DeletePartenaireCommandValidator : AbstractValidator<DeletePartenaireCommand>
{
    public DeletePartenaireCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant est requis.");
    }
}
