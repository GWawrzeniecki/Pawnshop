using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.ContractItemViews.ViewModels;
using PawnShop.Core.Constants;
using PawnShop.Core.ViewModel;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Laptop = PawnShop.Controls.ContractItemViews.Views.Laptop;

namespace PawnShop.Modules.Contract.Dialogs.ViewModels
{
    public class AddContractItemDialogViewModel : ViewModelBase<AddContractItemDialogViewModel>, IDialogAware
    {

        #region PrivateMembers

        private string _title;
        private string _contractItemName;
        private int _contractItemQuantity;
        private string _contractItemDescription;
        private string _contractItemTechnicalCondition;
        private decimal _contractItemEstimatedValueDecimal;
        private IList<ContractItemCategory> _contractItemCategories;
        private IList<UnitMeasure> _contractItemUnitMeasures;
        private ContractItemCategory _selectedContractItemCategory;
        private UnitMeasure _selectedContractItemUnitMeasure;
        private UserControl _additionalInformationUserControl;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _addContractItemCommand;
        private IList<ContractItemState> _contractItemStates;
        private ContractItemState _selectedContractItemState;
        private readonly IContractItemService _contractItemService;
        private readonly IContainerProvider _containerProvider;
        private readonly IMapper _mapper;
        private readonly IMessageBoxService _messageBoxService;

        #endregion

        #region Constructor

        public AddContractItemDialogViewModel(IContractItemService contractItemService, AddContractItemValidator addContractItemValidator,
            IContainerProvider containerProvider, IMapper mapper, IMessageBoxService messageBoxService) : base(addContractItemValidator)
        {
            _contractItemService = contractItemService;
            _containerProvider = containerProvider;
            _mapper = mapper;
            _messageBoxService = messageBoxService;
            Title = "Dodanie towaru do umowy";
            LoadStartupData();

        }

        #endregion

        #region PublicProperties
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string ContractItemName
        {
            get => _contractItemName;
            set => SetProperty(ref _contractItemName, value);
        }

        public int ContractItemQuantity
        {
            get => _contractItemQuantity;
            set => SetProperty(ref _contractItemQuantity, value);
        }

        public string ContractItemDescription
        {
            get => _contractItemDescription;
            set => SetProperty(ref _contractItemDescription, value);
        }

        public string ContractItemTechnicalCondition
        {
            get => _contractItemTechnicalCondition;
            set => SetProperty(ref _contractItemTechnicalCondition, value);
        }

        public decimal ContractItemEstimatedValue
        {
            get => _contractItemEstimatedValueDecimal;
            set => SetProperty(ref _contractItemEstimatedValueDecimal, value);
        }


        public IList<ContractItemCategory> ContractItemCategories
        {
            get => _contractItemCategories;
            set => SetProperty(ref _contractItemCategories, value);
        }

        public IList<UnitMeasure> ContractItemUnitMeasures
        {
            get => _contractItemUnitMeasures;
            set => SetProperty(ref _contractItemUnitMeasures, value);
        }


        public ContractItemCategory SelectedContractItemCategory
        {
            get => _selectedContractItemCategory;
            set
            {
                SetProperty(ref _selectedContractItemCategory, value);
                SetAdditionalInformationUserControl(value);
                (AdditionalInformationUserControl.DataContext as IRaiseCanExecuteChanged).Commands.Add(AddContractItemCommand);
            }
        }


        public UnitMeasure SelectedContractItemUnitMeasure
        {
            get => _selectedContractItemUnitMeasure;
            set => SetProperty(ref _selectedContractItemUnitMeasure, value);
        }


        public UserControl AdditionalInformationUserControl
        {
            get => _additionalInformationUserControl;
            set => SetProperty(ref _additionalInformationUserControl, value);
        }

        public bool? DetailedInformationUserControlHasErrors => (GetAdditionalInformationDataContext() as IViewModelBase)?.HasErrors;



        public IList<ContractItemState> ContractItemStates
        {
            get => _contractItemStates;
            set => SetProperty(ref _contractItemStates, value);
        }

        public ContractItemState SelectedContractItemState
        {
            get => _selectedContractItemState;
            set => SetProperty(ref _selectedContractItemState, value);
        }

        #endregion

        #region commands

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);


        public DelegateCommand AddContractItemCommand =>
            _addContractItemCommand ??=
                new DelegateCommand(AddContractItem, CanExecuteAddContractItem)
            .ObservesProperty(() => HasErrors);

        #endregion

        #region IDialogAware

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //Title = parameters.GetValue<string>("title");
        }


        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region viewModelBase

        protected override AddContractItemDialogViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadContractItemCategories();
                await TryToLoadUnitMeasures();
                await TryToLoadContractItemStates();
            }
            catch (LoadingContractItemCategoriesException loadingContractItemCategoriesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractItemCategoriesException.Message}{Environment.NewLine}Błąd: {loadingContractItemCategoriesException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingUnitMeasuresException loadingUnitMeasuresException)
            {
                _messageBoxService.ShowError(
                    $"{loadingUnitMeasuresException.Message}{Environment.NewLine}Błąd: {loadingUnitMeasuresException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingContractItemStatesException loadingContractItemStatesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractItemStatesException.Message}{Environment.NewLine}Błąd: {loadingContractItemStatesException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }


        private async Task TryToLoadContractItemCategories()
        {
            ContractItemCategories = await _contractItemService.GetContractItemCategories();
        }

        private async Task TryToLoadUnitMeasures()
        {
            ContractItemUnitMeasures = await _contractItemService.GetUnitMeasures();
        }

        private async Task TryToLoadContractItemStates()
        {
            ContractItemStates = await _contractItemService.GetContractItemStates();
        }

        private void SetAdditionalInformationUserControl(ContractItemCategory contractItemCategory)
        {
            AdditionalInformationUserControl = contractItemCategory.Category switch
            {
                Constants.Laptop => _containerProvider.Resolve<Laptop>(),
                _ => throw new NotImplementedException(contractItemCategory.Category)
            };
        }

        private object GetAdditionalInformationDataContext()
        {
            return AdditionalInformationUserControl?.DataContext;
        }



        #endregion

        #region commandMethods

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void AddContractItem()
        {
            var contractItem = new ContractItem();
            contractItem = _mapper.Map(this, contractItem);
            var additionalInformationDataContext = GetAdditionalInformationDataContext();
            if (additionalInformationDataContext != null)
            {
                switch (additionalInformationDataContext)
                {
                    case LaptopViewModel laptopViewModel:
                        var laptop = _mapper.Map<Business.Models.Laptop>(laptopViewModel);
                        contractItem.Laptop = laptop;
                        break;
                }

            }

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK,
                    new DialogParameters { { "contractItem", contractItem
    }
    }));
        }

        private bool CanExecuteAddContractItem() => !HasErrors && DetailedInformationUserControlHasErrors.HasValue && !DetailedInformationUserControlHasErrors.Value;


        #endregion
    }
}