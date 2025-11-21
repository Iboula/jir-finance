using MediatR;

namespace JIR.Application.Magasins.Queries.GetMagasins;

public class GetMagasinsQuery : IRequest<GetMagasinsQueryResult>
{
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
