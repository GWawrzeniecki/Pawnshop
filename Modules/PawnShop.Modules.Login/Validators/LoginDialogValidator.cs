using FluentValidation;
using PawnShop.Modules.Login.ViewModels;
using PawnShop.Validator.Base;

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