using MeDesenvolveIA.API.Extensions;
using MeDesenvolveIA.Shareable.Requests;
using MeDesenvolveIA.Shareable.Responses;
using MediatR;

namespace MeDesenvolveIA.API.Endpoints;

public static class PDIEndPoints
{
	public static void MapPDIEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/v1/pdis")
					   .WithTags("PDI");

		group.MapPost("", async (CriarPdiRequest request, IMediator mediator)
			=> await mediator.SendCommand(request))
				.WithName("CriarPdi")
				.WithDisplayName("Criação de PDI com IA")
				.WithSummary("Criação de PDI")
				.WithDescription("Gera um plano de desenvolvimento individual (PDI) com base em informações fornecidas.")
				.Produces<CriarPdiResponse>(200);
	}
}
