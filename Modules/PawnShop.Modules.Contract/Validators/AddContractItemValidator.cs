﻿using FluentValidation;
using PawnShop.Core;
using PawnShop.Modules.Contract.Dialogs.ViewModels;

namespace PawnShop.Modules.Contract.Validators
{
    public class AddContractItemValidator : ValidatorBase<AddContractItemDialogViewModel>
    {


        public AddContractItemValidator()
        {
            RuleFor(view => view.ContractItemName)
                 .NotEmpty()
                 .WithMessage("Pole nie posiada wartości.")
                 .MaximumLength(30)
                 .WithMessage("Maksymalnie 30 znaków.");

            RuleFor(view => view.ContractItemDescription)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                 .MaximumLength(100)
             .WithMessage("Maksymalnie 100 znaków.");

            RuleFor(view => view.SelectedContractItemCategory)
              .NotNull()
              .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.SelectedContractItemState)
                .NotNull()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.ContractItemTechnicalCondition)
             .NotEmpty()
             .WithMessage("Pole nie posiada wartości.")
             .MaximumLength(100)
             .WithMessage("Maksymalnie 100 znaków.");

            RuleFor(view => view.SelectedContractItemUnitMeasure)
             .NotNull()
             .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.ContractItemQuantity)
               .NotEmpty()
               .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.ContractItemEstimatedValue)
              .NotEmpty()
              .WithMessage("Pole nie posiada wartości.");

        }
    }
}
