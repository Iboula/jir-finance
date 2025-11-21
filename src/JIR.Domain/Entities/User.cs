using JIR.Domain.Common;
using JIR.Domain.Enums;

namespace JIR.Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid? SectionId { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public Section? Section { get; set; }
    public ICollection<Section> ManagedSections { get; set; } = new List<Section>();
}
