using FluentValidation;
using PawnShop.Controls.Dialogs.ViewModels;
using PawnShop.Services.Interfaces;
using PawnShop.Validator.Base;

namespace PawnShop.Controls.Validators
{
    public class AddClientValidator : ValidatorBase<AddClientDialogViewModel>
    {
        private readonly IValidatorService _validatorService;

        public AddClientValidator(IValidatorService validatorService)
        {

            RuleFor(view => view.FirstName)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.FirstName)
                        .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                        .WithMessage("Nieprawidłowe imię klienta.");
                });

            RuleFor(view => view.LastName)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.LastName)
                        .Matches(@"^\p{Lu}{1}\D+$")
                        .When(view => !string.IsNullOrEmpty(view.LastName))
                        .WithMessage("Nieprawidłowe nazwisko klienta.");
                });

            RuleFor(view => view.Street)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.Street)
                        .Matches(@"^(\d\s)?(\p{Lu}{1}\p{Ll}+){1}([\s-]\p{Lu}{1}\p{Ll}+)*$")
                        .When(view => !string.IsNullOrEmpty(view.Street))
                        .WithMessage("Nieprawidłowe nazwa ulicy.");
                });

            RuleFor(view => view.City)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.City)
                        .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                        .When(view => !string.IsNullOrEmpty(view.City))
                        .WithMessage("Nieprawidłowa nazwa miejscowości.");
                });
            RuleFor(view => view.Country)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.Country)
                        .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                        .When(view => !string.IsNullOrEmpty(view.Country))
                        .WithMessage("Nieprawidłowa nazwa kraju.");
                });
            RuleFor(view => view.PostCode)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.PostCode)
                        .Matches(@"^\d{2}-\d{3}$")
                        .When(view => !string.IsNullOrEmpty(view.PostCode))
                        .WithMessage("Nieprawidłowy format kodu pocztowego.");
                });


            RuleFor(view => view.Pesel)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.Pesel)
                        .Matches(@"^\d{11}$")
                        .When(view => !string.IsNullOrEmpty(view.Pesel))
                        .WithMessage("Nieprawidłowy format peselu.");
                });


            RuleFor(view => view.IdCardNumber)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.IdCardNumber)
                        .Must(BeAValidIdCard)
                        .When(view => !string.IsNullOrEmpty(view.IdCardNumber))
                        .WithMessage("Nieprawidłowy format numeru dowodu.");
                });

            RuleFor(view => view.BirthDate)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.ValidityDateIdCard)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.HouseNumber)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");
            _validatorService = validatorService;
        }

        private bool BeAValidIdCard(string arg)
        {
            return _validatorService.ValidateIdCardNumber(arg);
        }
    }
}