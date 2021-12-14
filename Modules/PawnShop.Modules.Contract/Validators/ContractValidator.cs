using FluentValidation;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Validator.Base;

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
                .Matches(@"^(\p{Lu}?\p{Ll}+){1}((\s)\p{Lu}?\p{Ll}+)*$")
                .When(contract => !string.IsNullOrEmpty(contract.Client))
                .WithMessage("Nieprawidłowa nazwa klienta.");
        }
    }
}