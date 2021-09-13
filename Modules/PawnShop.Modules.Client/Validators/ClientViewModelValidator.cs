using FluentValidation;
using PawnShop.Modules.Client.ViewModels;
using PawnShop.Services.Interfaces;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Client.Validators
{
    public class ClientViewModelValidator : ValidatorBase<ClientViewModel>
    {
        private readonly IValidatorService _validatorService;

        public ClientViewModelValidator(IValidatorService validatorService)
        {
            _validatorService = validatorService;
            RuleFor(clientViewModel => clientViewModel.ContractNumber)
                .Matches(@"^\d{2,5}/\d{4}$")
                .When(clientViewModel => !string.IsNullOrEmpty(clientViewModel.ContractNumber))
                .WithMessage("Nieprawidłowy format numeru umowy.");

            RuleFor(view => view.Pesel)
                .Matches(@"^\d{11}$")
                .When(view => !string.IsNullOrEmpty(view.Pesel))
                .WithMessage("Nieprawidłowy format peselu.");

            RuleFor(view => view.FirstName)
                .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                .When(view => !string.IsNullOrEmpty(view.FirstName))
                .WithMessage("Nieprawidłowe imię klienta.");

            RuleFor(view => view.LastName)
                .Matches(@"^\p{Lu}{1}\D+$")
                .When(view => !string.IsNullOrEmpty(view.LastName))
                .WithMessage("Nieprawidłowe nazwisko klienta.");

            RuleFor(view => view.Street)
                .Matches(@"^\p{Lu}{1}\D+$")
                .When(view => !string.IsNullOrEmpty(view.Street))
                .WithMessage("Nieprawidłowe nazwa ulicy.");

            RuleFor(view => view.IdCardNumber)
                .Must(BeAValidIdCard)
                .When(view => !string.IsNullOrEmpty(view.IdCardNumber))
                .WithMessage("Nieprawidłowy format numeru dowodu.");
        }
        private bool BeAValidIdCard(string arg)
        {
            return _validatorService.ValidateIdCardNumber(arg);
        }
    }
}