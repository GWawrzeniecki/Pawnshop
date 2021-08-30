using FluentValidation;
using PawnShop.Modules.Settings.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Settings.Validators
{
    public class PawnShopSettingsValidator : ValidatorBase<PawnShopSettingsViewModel>
    {
        public PawnShopSettingsValidator()
        {
            RuleFor(view => view.AutomaticSearchingEndedContractsDay)
                .Must(d => d > 0)
                .WithMessage("Ilość dni musi być większa niz 0");
        }
    }

}