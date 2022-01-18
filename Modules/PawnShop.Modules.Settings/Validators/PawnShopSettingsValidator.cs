using FluentValidation;
using PawnShop.Modules.Settings.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Settings.Validators
{
    public class PawnShopSettingsValidator : ValidatorBase<PawnShopSettingsViewModel>
    {
        public PawnShopSettingsValidator()
        {
            RuleFor(view => view.VatPercent)
                .Must(d => d > 0)
                .WithMessage("Stawka vatu musi być większa niż 0");
        }
    }

}