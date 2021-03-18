using FluentValidation;

namespace BistHub.Api.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> ThrowException<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorCode, string errorMessage)
        {
            return rule
                .OnAnyFailure(x => throw new Exceptions.ValidationException(errorCode, errorMessage));
        }
    }
}
