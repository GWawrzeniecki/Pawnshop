﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using PawnShop.Core;
using PawnShop.Modules.Contract.ViewModels;

namespace PawnShop.Modules.Contract.Validators
{
    public class ContractValidator : ValidatorBase<ContractViewModel>
    {
        public ContractValidator()
        {
            RuleFor(contract => contract.ContractNumber)
                .Matches(@"^\d{2,5}/\d{4}$")
                .When(contract => !string.IsNullOrEmpty(contract.ContractNumber))
                .WithMessage("Nieprawidłowy format numeru umowy.");


            RuleFor(contract => contract.ContractAmount)
                .Matches("^[0-9]+$")
                .When(contract => !string.IsNullOrEmpty(contract.ContractAmount))
                .WithMessage("Nieprawidłowy format kwoty umowy.");

            RuleFor(contract => contract.Client)
                .Matches("^[a-zA-Z]+([ ]?[a-zA-Z])*$")
                .When(contract => !string.IsNullOrEmpty(contract.Client))
                .WithMessage("Nieprawidłowe nazwa klienta");
        }
    }
}