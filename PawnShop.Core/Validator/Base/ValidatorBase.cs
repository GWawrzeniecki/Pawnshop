using FluentValidation;

namespace PawnShop.Validator.Base
{
    public abstract class ValidatorBase<T> : AbstractValidator<T> where T : class
    {

    }
}