using MeDesenvolveIA.API.Endpoints;
using MeDesenvolveIA.API.Middlewares;

namespace MeDesenvolveIA.API.Extensions;

public static class AppConfigureExtensions
{
	public static void Configure(this WebApplication app)
	{
		app.UseMiddleware<ExceptionMiddleware>();

		app.MapPDIEndpoints();

		app.MapOpenApi();
		app.UseSwagger();
		app.UseSwaggerUI();
	}
}