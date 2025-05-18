using MeDesenvolveIA.Shareable.Responses;
using MediatR;
using OperationResult;

namespace MeDesenvolveIA.Shareable.Requests;
public record CriarPdiRequest(
	string Nome,
	string CargoAtual,
	string CargoDesejado,
	string? EmpresaAlvo = null,
	List<string>? HabilidadesFortes = null,
	List<string>? HabilidadesADesenvolver = null
) : IRequest<Result<CriarPdiResponse>>, IValidatableRequest;
