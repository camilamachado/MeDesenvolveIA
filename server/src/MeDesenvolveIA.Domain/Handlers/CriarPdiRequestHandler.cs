using GenerativeAI;
using GenerativeAI.Web;
using MeDesenvolveIA.Shareable.Exceptions;
using MeDesenvolveIA.Shareable.Requests;
using MeDesenvolveIA.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;
using System.Text;
using System.Text.Json;

namespace MeDesenvolveIA.Domain.Handlers;

public sealed class CriarPdiRequestHandler : IRequestHandler<CriarPdiRequest, Result<CriarPdiResponse>>
{
	private readonly IGenerativeAiService _generativeAiService;
	private readonly ILogger<CriarPdiRequestHandler> _logger;

	public CriarPdiRequestHandler(IGenerativeAiService generativeAiService, ILogger<CriarPdiRequestHandler> logger)
	{
		_generativeAiService = generativeAiService;
		_logger = logger;
	}

	public async Task<Result<CriarPdiResponse>> Handle(CriarPdiRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var promptBuilder = new StringBuilder();

			promptBuilder.AppendLine($"Gere um Plano de Desenvolvimento Individual (PDI) para a pessoa {request.Nome}, que atualmente ocupa o cargo de {request.CargoAtual} e deseja alcançar o cargo de {request.CargoDesejado}.");

			if (!string.IsNullOrWhiteSpace(request.EmpresaAlvo))
			{
				promptBuilder.AppendLine($"A pessoa deseja alcançar esse cargo na empresa ou tipo de empresa: {request.EmpresaAlvo}. Considere os requisitos típicos dessas vagas.");
			}

			if (request.HabilidadesFortes?.Any() == true)
			{
				promptBuilder.AppendLine($"Ela já possui como habilidades bem desenvolvidas: {string.Join(", ", request.HabilidadesFortes)}.");
			}

			if (request.HabilidadesADesenvolver?.Any() == true)
			{
				promptBuilder.AppendLine($"Ela acredita que precisa desenvolver as seguintes habilidades: {string.Join(", ", request.HabilidadesADesenvolver)}. Leve isso em consideração.");
			}
			else
			{
				promptBuilder.AppendLine("Mesmo que a pessoa não saiba exatamente quais habilidades precisa desenvolver, identifique as principais lacunas com base na transição de cargo.");
			}

			promptBuilder.AppendLine(@"
			Com base nessas informações, gere um plano estruturado com:
			- Um objetivo geral claro
			- Objetivos SMART bem definidos
			- Ações organizadas por trimestre
			- Recursos necessários
			- Métricas de sucesso
			- Observações adicionais

			Retorne a resposta em formato JSON, conforme o modelo abaixo, sem formatações especiais (como \n, markdown, ou aspas extras):

			{
			  ""titulo"": ""..."",
			  ""nome"": ""..."",
			  ""cargoAtual"": ""..."",
			  ""cargoDesejado"": ""..."",
			  ""empresaAlvo"": ""..."",
			  ""objetivoGeral"": ""..."",
			  ""objetivosSmart"": [
				{
				  ""objetivo"": ""..."",
				  ""descricao"": ""..."",
				  ""specific"": ""..."",
				  ""measurable"": ""..."",
				  ""achievable"": ""..."",
				  ""relevant"": ""..."",
				  ""timeBound"": ""...""
				}
			  ],
			  ""planoTrimestral"": [
				{
				  ""trimestre"": ""..."",
				  ""foco"": ""..."",
				  ""acoes"": [""..."", ""...""]
				}
			  ],
			  ""recursosNecessarios"": [""..."", ""...""],
			  ""metricasDeSucesso"": [""..."", ""...""],
			  ""observacoes"": [""..."", ""...""]
			}");

			var prompt = promptBuilder.ToString();

			var model = _generativeAiService.CreateInstance(GoogleAIModels.Gemini2Flash);
			var response = await model.GenerateContentAsync(prompt).ConfigureAwait(false);

			var textoGerado = response.Text();
			if (!string.IsNullOrWhiteSpace(textoGerado))
			{
				try
				{
					var jsonLimpo = textoGerado
						.Replace("```json", "")
						.Replace("```", "")
						.Trim();

					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					};

					var pdi = JsonSerializer.Deserialize<CriarPdiResponse>(jsonLimpo, options);
					if (pdi is not null)
					{
						return pdi;
					}

					_logger.LogWarning("Não foi possível converter o texto gerado pela IA para CriarPdiResponse.");
				}
				catch (JsonException jsonEx)
				{
					_logger.LogError(jsonEx, "Falha na desserialização do texto gerado pela IA.");
				}
			}

			_logger.LogWarning("Texto gerado pela IA está vazio ou nulo.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Erro ao gerar o PDI");
		}

		return new UnexpectedApplicationException("Ocorreu um erro ao gerar o PDI.");
	}
}