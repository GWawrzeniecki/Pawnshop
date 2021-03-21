using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using PawnShop.Core;
using PawnShop.Modules.Login.ViewModels;

namespace PawnShop.Modules.Login.Validators
{
    public class LoginDialogValidator : ValidatorBase<LoginDialogViewModel>
    {
        public LoginDialogValidator()
        {
            RuleFor(login => login.PasswordTag)
                .Must(BeAValidCredentials)
                .When(login => login.UserNameHasText && login.PasswordBoxHasText)
                .WithMessage("Login lub  hasło jest nieprawidłowe.");
        }

        private bool BeAValidCredentials(bool arg)
        {
            return arg;
        }
    }
}
