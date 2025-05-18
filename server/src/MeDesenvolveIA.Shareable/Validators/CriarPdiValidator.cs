using FluentValidation;
using MeDesenvolveIA.Shareable.Requests;

public class CriarPdiValidator : AbstractValidator<CriarPdiRequest>
{
	public CriarPdiValidator()
	{
		RuleFor(x => x.Nome)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.CargoAtual)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.CargoDesejado)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.HabilidadesFortes)
			.NotNull()
			.Must(x => x.Any());

		RuleFor(x => x.EmpresaAlvo)
			.MaximumLength(200)
			.When(x => !string.IsNullOrWhiteSpace(x.EmpresaAlvo));
	}
}
