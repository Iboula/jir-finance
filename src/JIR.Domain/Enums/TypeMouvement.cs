using System.Text.Json.Serialization;

namespace JIR.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TypeMouvement
{
    Entree,
    Sortie,
    Ajustement,
    Transfert
}
