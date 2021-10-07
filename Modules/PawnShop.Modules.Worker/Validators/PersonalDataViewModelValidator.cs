using FluentValidation;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Dialogs.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Worker.Validators
{
    public class PersonalDataViewModelValidator : ValidatorBase<WorkerDialogBase>
    {
        public PersonalDataViewModelValidator()
        {
            RuleFor(view => (view as PersonalDataViewModel).FirstName)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).FirstName)
                        .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                        .WithMessage("Nieprawidłowe imię klienta.");
                });

            RuleFor(view => (view as PersonalDataViewModel).LastName)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).LastName)
                        .Matches(@"^\p{Lu}{1}\D+$")
                        .When(view => !string.IsNullOrEmpty((view as PersonalDataViewModel).LastName))
                        .WithMessage("Nieprawidłowe nazwisko klienta.");
                });

            RuleFor(view => (view as PersonalDataViewModel).Street)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).Street)
                        .Matches(@"^\p{Lu}{1}\D+$")
                        .When(view => !string.IsNullOrEmpty((view as PersonalDataViewModel).Street))
                        .WithMessage("Nieprawidłowe nazwa ulicy.");
                });

            RuleFor(view => (view as PersonalDataViewModel).City)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).City)
                        .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                        .When(view => !string.IsNullOrEmpty((view as PersonalDataViewModel).City))
                        .WithMessage("Nieprawidłowa nazwa miejscowości.");
                });
            RuleFor(view => (view as PersonalDataViewModel).Country)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).Country)
                        .Matches(@"^\p{Lu}{1}\p{Ll}+$")
                        .When(view => !string.IsNullOrEmpty((view as PersonalDataViewModel).Country))
                        .WithMessage("Nieprawidłowa nazwa kraju.");
                });
            RuleFor(view => (view as PersonalDataViewModel).PostCode)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).PostCode)
                        .Matches(@"^\d{2}-\d{3}$")
                        .When(view => !string.IsNullOrEmpty((view as PersonalDataViewModel).PostCode))
                        .WithMessage("Nieprawidłowy format kodu pocztowego.");
                });


            RuleFor(view => (view as PersonalDataViewModel).Pesel)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .DependentRules(() =>
                {
                    RuleFor(view => (view as PersonalDataViewModel).Pesel)
                        .Matches(@"^\d{11}$")
                        .When(view => !string.IsNullOrEmpty((view as PersonalDataViewModel).Pesel))
                        .WithMessage("Nieprawidłowy format peselu.");
                });

            RuleFor(view => (view as PersonalDataViewModel).BirthDate)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => (view as PersonalDataViewModel).HouseNumber)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");
        }
    }
}
