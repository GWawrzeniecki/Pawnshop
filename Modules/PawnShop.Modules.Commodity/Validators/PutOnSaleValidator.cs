using FluentValidation;
using PawnShop.Modules.Commodity.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Commodity.Validators
{
    public class PutOnSaleValidator : ValidatorBase<PutOnSaleViewModel>
    {
        public PutOnSaleValidator()
        {
            RuleFor(view => view.SelectedContractItemUnitMeasure)
                .NotNull()
                .WithMessage("Pole nie posiada wartoci");

            RuleFor(view => view.ContractItemQuantity)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.ContractItemQuantity)
                .LessThanOrEqualTo(view => view.ContractItemQuantityForSale)
                .WithMessage("Produkt jest dostepny w mniejszej ilości");

            RuleFor(view => view.Price)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => view.Rack)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .MaximumLength(10)
                .WithMessage("Maksymalnie 10 znaków.");

            RuleFor(view => view.Shelf)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            When(view => !string.IsNullOrEmpty(view.SaleLinkText), () =>
            {
                RuleFor(view => view.SaleLinkText)
                    .Must(s => !string.IsNullOrWhiteSpace(s))
                    .WithMessage("Link nie może zawierać białych znaków");
            });

        }
    }
}