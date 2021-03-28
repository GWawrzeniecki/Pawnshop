using FluentValidation;

namespace PawnShop.Core
{
    public abstract class ValidatorBase<T> : AbstractValidator<T> where T : class
    {
    }
}