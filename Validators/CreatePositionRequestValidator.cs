using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using FluentValidation;
using System;

namespace BistHub.Api.Validators
{
	public class CreatePositionRequestValidator : AbstractValidator<CreatePositionRequest>
	{
		public CreatePositionRequestValidator()
		{
			RuleFor(x => x.Amount)
				.GreaterThan(0L)
				.ThrowException(Errors.PositionAmountMustBePositive, "Pozisyon büyüklüğü 0'dan büyük olmalı.");

			RuleFor(x => x.BuyDate)
				.LessThanOrEqualTo(DateTime.Today)
				.ThrowException(Errors.PositionBuyDateMustPassed, "Alış tarihi bugünden ileri olamaz.");

			RuleFor(x => x.BuyPrice)
				.GreaterThan(0m)
				.ThrowException(Errors.PositionBuyPriceMustBePositive, "Alış fiyatı 0'dan büyük olmalı.");

			RuleFor(x => x.PaidFee)
				.GreaterThanOrEqualTo(0m)
				.ThrowException(Errors.PositionPaidFeeCannotBeNegative, "Alış fiyatı 0'dan büyük olmalı.");
		}
	}
}
