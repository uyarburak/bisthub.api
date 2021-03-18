using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using FluentValidation;
using System;

namespace BistHub.Api.Validators
{
    public class UpdatePositionRequestValidator : AbstractValidator<UpdatePositionRequest>
    {
        public UpdatePositionRequestValidator()
        {
            Include(new CreatePositionRequestValidator());

            RuleFor(x => x.SellDate)
                .LessThanOrEqualTo(DateTime.Today)
                .When(x => x.SellDate.HasValue)
                .ThrowException(Errors.PositionSellDateMustPassed, "Satış tarihi bugünden ileri olamaz.");

            RuleFor(x => x.SellPrice)
                .GreaterThan(0m)
                .When(x => x.SellPrice.HasValue)
                .ThrowException(Errors.PositionSellPriceMustBePositive, "Satış fiyatı 0'dan büyük olmalı.");

            RuleFor(x => x.SellDate)
                .NotNull()
                .When(x => x.SellPrice.HasValue)
                .ThrowException(Errors.PositionSellPriceAndDateNotFound, "Satış tarihi ve fiyatını beraber giriniz.");

            RuleFor(x => x.SellPrice)
                .NotNull()
                .When(x => x.SellDate.HasValue)
                .ThrowException(Errors.PositionSellPriceAndDateNotFound, "Satış tarihi ve fiyatını beraber giriniz.");
        }
    }
}
