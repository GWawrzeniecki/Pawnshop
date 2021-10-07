﻿using FluentValidation;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Dialogs.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Worker.Validators
{
    public class LoginPrivilegesDataViewModelValidator : ValidatorBase<WorkerDialogBase>
    {
        public LoginPrivilegesDataViewModelValidator()
        {
            RuleFor(view => (view as LoginPrivilegesDataViewModel).UserLogin)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => (view as LoginPrivilegesDataViewModel).PasswordBoxHasText)
                .Must(k => k)
                .WithMessage("Pole nie posiada wartości.");
        }
    }
}