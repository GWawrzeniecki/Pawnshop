using FluentValidation;
using PawnShop.Modules.Commodity.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Commodity.Validators
{
    public class CommodityValidator : ValidatorBase<CommodityViewModel>
    {
        public CommodityValidator()
        {
            RuleFor(contract => contract.ContractNumber)
                .Matches(@"^\d{2,5}/\d{4}$")
                .When(contract => !string.IsNullOrEmpty(contract.ContractNumber))
                .WithMessage("Nieprawidłowy format numeru umowy.");

            RuleFor(contract => contract.Price)
                .Matches("^[0-9]+$")
                .When(contract => !string.IsNullOrEmpty(contract.Price))
                .WithMessage("Nieprawidłowy format kwoty przedmiotu.");

            RuleFor(contract => contract.Client)
                .Matches("^[a-zA-Z]+([ ]?[a-zA-Z])*$")
                .When(contract => !string.IsNullOrEmpty(contract.Client))
                .WithMessage("Nieprawidłowa nazwa klienta.");
        }
    }
}