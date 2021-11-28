using FluentValidation;
using PawnShop.Modules.Sale.Dialogs.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Sale.Validators
{
    public class SellDialogValidator : ValidatorBase<SellDialogViewModel>
    {
        public SellDialogValidator()
        {
            RuleFor(v => v.SelectedPaymentType)
                .NotNull()
                .WithMessage("Brak wybranego typu płatności");
        }
    }
}