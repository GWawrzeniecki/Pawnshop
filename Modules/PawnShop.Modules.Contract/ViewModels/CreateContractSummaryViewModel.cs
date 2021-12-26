using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.Constants;
using PawnShop.Core.Events;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Windows.ViewModels;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class CreateContractSummaryViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {

        #region PrivateMembers

        private readonly ICalculateService _calculateService;
        private readonly ISessionContext _sessionContext;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;
        private readonly IShellService _shellService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageBoxService _messageBoxService;
        private Business.Models.Contract _contract;
        private bool _isPrintDealDocument;
        private DelegateCommand _createContractCommand;
        private Func<Task> _callBack;

        #endregion

        #region Constructor

        public CreateContractSummaryViewModel(ICalculateService calculateService, ISessionContext sessionContext,
             IContractService contractService, IMapper mapper, IShellService shellService, IEventAggregator eventAggregator, IMessageBoxService messageBoxService)
        {
            _calculateService = calculateService;
            _sessionContext = sessionContext;
            _contractService = contractService;
            _mapper = mapper;
            _shellService = shellService;
            _eventAggregator = eventAggregator;
            _messageBoxService = messageBoxService;
            Contract = new Business.Models.Contract();
        }
        #endregion

        #region PublicProperties


        public Business.Models.Contract Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }


        public bool IsPrintDealDocument
        {
            get => _isPrintDealDocument;
            set => SetProperty(ref _isPrintDealDocument, value);
        }

        public decimal SumOfEstimatedValues => Contract.ContractItems.Sum(c => c.EstimatedValue);

        public decimal RePurchasePrice =>
            _calculateService.CalculateContractAmount(SumOfEstimatedValues, Contract.LendingRate);

        public decimal NetStorageCost =>
            _calculateService.CalculateNetStorageCost(SumOfEstimatedValues, Contract.LendingRate);

        public decimal PCC => SumOfEstimatedValues >= 1000 ? SumOfEstimatedValues * 2 / 100 : 0;

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region Commands

        public DelegateCommand CreateContractCommand =>
            _createContractCommand ??= new DelegateCommand(CreateContract);

        #endregion

        #region CommandMethods

        private async void CreateContract()
        {
            try
            {
                if (_shellService.GetShellViewModel<CreateContractWindow>() is CreateContractWindowViewModel vm)
                    vm.IsBusy = true;
                await TryToInsertContractAsync();
                _eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Publish();
                _messageBoxService.Show("Pomyślnie utworzono umowę.", "Sukces");
                if (IsPrintDealDocument)
                    await TryToPrintDealDocumentAsync();
                if (_callBack is not null)
                    await _callBack.Invoke();
            }
            catch (CreateContractException createContractException)
            {
                _messageBoxService.ShowError(
                    $"{createContractException.Message}{Environment.NewLine}Błąd: {createContractException.InnerException?.Message}",
                    "Błąd");
            }
            catch (PrintDealDocumentException printDealDocumentException)
            {
                _messageBoxService.ShowError(
                    $"{printDealDocumentException.Message}{Environment.NewLine}Błąd: {printDealDocumentException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
            finally
            {
                if (_shellService.GetShellViewModel<CreateContractWindow>() is CreateContractWindowViewModel vm)
                    vm.IsBusy = false;
                _shellService.CloseShell<CreateContractWindow>();
            }
        }

        #endregion

        #region PrivateMethods

        private async Task TryToInsertContractAsync()
        {
            var insertContract = _mapper.Map<InsertContract>(Contract);
            await _contractService.CreateContract(insertContract, Constants.CashPaymentType, SumOfEstimatedValues, DateTime.Now, SumOfEstimatedValues);
        }

        private async Task TryToPrintDealDocumentAsync()
        {
            await _contractService.PrintDealDocument(Contract);
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var contractItems = navigationContext.Parameters.GetValue<IList<ContractItem>>("ContractItems");
            if (contractItems is not null && contractItems.Any())
                Contract.ContractItems = new Collection<ContractItem>(contractItems);
            Contract.LendingRate = navigationContext.Parameters.GetValue<LendingRate>("LendingRate") ?? Contract.LendingRate;
            Contract.LendingRateId = Contract.LendingRate.Id;
            Contract.DealMaker = navigationContext.Parameters.GetValue<Client>("DealMaker") ?? Contract.DealMaker;
            Contract.DealMakerId = Contract.DealMaker.ClientId;
            var startDate = navigationContext.Parameters.GetValue<DateTime>("StartDate");
            if (DateTime.Compare(startDate, DateTime.MinValue) > 0)
                Contract.StartDate = startDate;
            Contract.ContractNumberId = navigationContext.Parameters.GetValue<string>("ContractNumber") ?? Contract.ContractNumberId;
            Contract.AmountContract = SumOfEstimatedValues;
            Contract.WorkerBossId = _sessionContext.LoggedPerson.WorkerBossId;
            _callBack = navigationContext.Parameters.GetValue<Func<Task>>("CallBack");
            RaisePropertyChanged(nameof(RePurchasePrice));
            RaisePropertyChanged(nameof(NetStorageCost));
            RaisePropertyChanged(nameof(SumOfEstimatedValues));
            RaisePropertyChanged(nameof(PCC));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion

    }
}