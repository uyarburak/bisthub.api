using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using FluentValidation;

namespace BistHub.Api.Validators
{
    public class CreatePortfolioRequestValidator : AbstractValidator<CreatePortfolioRequest>
    {
		public CreatePortfolioRequestValidator()
		{
			RuleFor(x => x.Title)
				.NotNull().NotEmpty().Length(1, 32)
				.ThrowException(Errors.InvalidPortfolioTitle, "Portföy başlığı 1-32 karakter uzunluğunda olmalı.");
		}
	}
}
