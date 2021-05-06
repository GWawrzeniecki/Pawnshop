﻿using FluentValidation;
using PawnShop.Core;
using PawnShop.Modules.Contract.Dialogs.ViewModels;
using PawnShop.Services.Interfaces;

namespace PawnShop.Modules.Contract.Validators
{
    public class AddClientValidator : ValidatorBase<AddClientDialogViewModel>
    {
        private readonly IValidatorService _validatorService;

        public AddClientValidator(IValidatorService validatorService)
        {
            _validatorService = validatorService;
            RuleFor(view => view.FirstName)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.FirstName)
                        .Matches("^[A-Z]{1}[a-z]+$")
                        .WithMessage("Nieprawidłowe imię klienta.");
                });

            RuleFor(view => view.LastName)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.LastName)
                        .Matches("^[A-Z]{1}[a-z]+$")
                        .When(view => !string.IsNullOrEmpty(view.LastName))
                        .WithMessage("Nieprawidłowe nazwisko klienta.");
                });

            RuleFor(view => view.Street)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.Street)
                        .Matches("^[A-Z]{1}[a-z]+$")
                        .When(view => !string.IsNullOrEmpty(view.Street))
                        .WithMessage("Nieprawidłowe nazwa ulicy.");
                });

            RuleFor(view => view.City)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.City)
                        .Matches("^[A-Z]{1}[a-z]+$")
                        .When(view => !string.IsNullOrEmpty(view.City))
                        .WithMessage("Nieprawidłowa nazwa miejscowości.");
                });
            RuleFor(view => view.Country)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => view.Country)
                        .Matches("^[A-Z]{1}[a-z]+$")
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
        }

        private bool BeAValidIdCard(string arg)
        {
            return _validatorService.ValidateIdCardNumber(arg);
        }
    }
}