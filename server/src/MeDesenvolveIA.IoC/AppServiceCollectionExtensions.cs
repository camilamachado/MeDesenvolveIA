using FluentValidation;
using GenerativeAI;
using GenerativeAI.Web;
using MeDesenvolveIA.Domain;
using MeDesenvolveIA.Shareable;
using MeDesenvolveIA.Shareable.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeDesenvolveIA.IoC;

public static class AppServiceCollectionExtensions
{
	public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssemblies(
				typeof(IDomainEntryPoint).Assembly,
				typeof(ValidationBehavior<,>).Assembly)
		);

		services.ConfigureGoogleGemini(configuration);

		services.AddValidatorsFromAssemblyContaining<IShareableEntryPoint>();
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
	}

	private static void ConfigureGoogleGemini(this IServiceCollection services, IConfiguration configuration)
	{
		var apiKey = configuration["GoogleGemini:ApiKey"] ?? throw new InvalidOperationException("A chave da API do Google Gemini não foi configurada.");

		services.AddGenerativeAI(new GenerativeAIOptions()
		{
			Model = GoogleAIModels.Gemini2Flash,
			Credentials = new GoogleAICredentials()
			{
				ApiKey = apiKey
			}

		}).WithAdc();
	}
}