using JIR.Domain.Enums;
using MediatR;

namespace JIR.Application.Partenaires.Commands.UpdatePartenaire;

public class UpdatePartenaireCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public TypePartenaire TypePartenaire { get; set; }
    public string? Adresse { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? ContactNom { get; set; }
    public string? Observations { get; set; }
    public bool IsActif { get; set; }
}
