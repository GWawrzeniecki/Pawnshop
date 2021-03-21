using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace PawnShop.Core
{
    public abstract class ValidatorBase<T> : AbstractValidator<T> where T : class
    {
        
    }
}
