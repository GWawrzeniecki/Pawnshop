using FluentValidation;
using PawnShop.Controls.ContractItemViews.ViewModels;
using PawnShop.Core;

namespace PawnShop.Controls.ContractItemViews.Validators
{
    public class LaptopValidator : ValidatorBase<LaptopViewModel>
    {
        public LaptopValidator()
        {
            RuleFor(view => view.Brand)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.")
                .MaximumLength(30)
                .WithMessage("Maksymalnie 30 znaków.");

            RuleFor(view => view.Processor)
               .NotEmpty()
               .WithMessage("Pole nie posiada wartości.")
               .MaximumLength(30)
               .WithMessage("Maksymalnie 30 znaków.");

            RuleFor(view => view.Ram)
               .NotEmpty()
               .WithMessage("Pole nie posiada wartości.")
               .MaximumLength(15)
               .WithMessage("Maksymalnie 15 znaków.");

            RuleFor(view => view.DriverType)
               .NotEmpty()
               .WithMessage("Pole nie posiada wartości.")
               .MaximumLength(10)
               .WithMessage("Maksymalnie 10 znaków.");

            RuleFor(view => view.MassStorage)
               .NotEmpty()
               .WithMessage("Pole nie posiada wartości.")
               .MaximumLength(15)
               .WithMessage("Maksymalnie 15 znaków.");

            RuleFor(view => view.DescriptionKit)
               .NotEmpty()
               .WithMessage("Pole nie posiada wartości.")
               .MaximumLength(100)
               .WithMessage("Maksymalnie 100 znaków.");
        }
    }
}
