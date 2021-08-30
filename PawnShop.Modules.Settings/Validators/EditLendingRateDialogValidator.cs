using FluentValidation;
using PawnShop.Modules.Settings.Dialogs.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Settings.Validators
{
    public class EditLendingRateDialogValidator : ValidatorBase<EditLendingRateDialogViewModel>
    {
        public EditLendingRateDialogValidator()
        {
            RuleFor(view => view.Days)
                .Must(d => d > 0)
                .When(view => view.HasDaysText)
                .WithMessage("Ilość dni musi być większa niz 0");

            RuleFor(view => view.Percentage)
                .Must(p => p > 0)
                .When(view => view.HasPercentageText)
                .WithMessage("Oprocentowanie musi być większe niz 0");
        }
    }
}