using FluentValidation;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Dialogs.ViewModels;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Worker.Validators
{
    public class WorkerDataViewModelValidator : ValidatorBase<WorkerDialogBase>
    {
        public WorkerDataViewModelValidator()
        {
            RuleFor(view => (view as WorkerDataViewModel).WorkerBossTypeStr)
                .NotEmpty()
                .WithMessage("Pole nie posiada wartości.");

            RuleFor(view => (view as WorkerDataViewModel).Salary)
                .NotNull()
                .WithMessage("Pole nie posiada wartości.");

        }
    }
}