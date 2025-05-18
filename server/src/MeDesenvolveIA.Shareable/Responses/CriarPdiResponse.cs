namespace MeDesenvolveIA.Shareable.Responses;

public record CriarPdiResponse(
	string Titulo,
	string Nome,
	string CargoAtual,
	string CargoDesejado,
	string? EmpresaAlvo,
	string ObjetivoGeral,
	List<ObjetivoSmart> ObjetivosSmart,
	List<TrimestrePlano> PlanoTrimestral,
	List<string> RecursosNecessarios,
	List<string> MetricasDeSucesso,
	List<string> Observacoes
);

public record ObjetivoSmart(
	string Objetivo,
	string Descricao,
	string Specific,
	string Measurable,
	string Achievable,
	string Relevant,
	string TimeBound
);

public record TrimestrePlano(
	string Trimestre,
	string Foco,
	List<string> Acoes
);
