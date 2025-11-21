using MediatR;

namespace JIR.Application.Cotisations.Commands.CreateCotisation;

public class CreateCotisationCommand : IRequest<Guid>
{
    public Guid SectionId { get; set; }
    public string MembreNom { get; set; } = string.Empty;
    public string MembreMatricule { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Periode { get; set; } = string.Empty;
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? RecuNumero { get; set; }
    public string Statut { get; set; } = "Validé";
    public string? Observations { get; set; }
}
