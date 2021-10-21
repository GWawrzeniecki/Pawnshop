using FluentValidation;
using PawnShop.Modules.Sale.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Sale.Validators
{
    public class SaleValidator : ValidatorBase<SaleViewModel>
    {
        public SaleValidator()
        {
            RuleFor(contract => contract.ContractNumber)
                .Matches(@"^\d{2,5}/\d{4}$")
                .When(contract => !string.IsNullOrEmpty(contract.ContractNumber))
                .WithMessage("Nieprawidłowy format numeru umowy.");

            RuleFor(contract => contract.Price)
                .Matches("^[0-9]+$")
                .When(contract => !string.IsNullOrEmpty(contract.Price))
                .WithMessage("Nieprawidłowy format ceny towaru.");

            RuleFor(contract => contract.Client)
                .Matches("^[a-zA-Z]+([ ]?[a-zA-Z])*$")
                .When(contract => !string.IsNullOrEmpty(contract.Client))
                .WithMessage("Nieprawidłowa nazwa klienta.");
        }
    }
}