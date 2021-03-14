using Prism.Mvvm;
using System.Collections.Generic;
using PawnShop.Business.Models;
using System;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Services;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using PawnShop.Core.Dialogs;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractViewModel : BindableBase
    {
        #region private members
        private List<Business.Models.Contract> _contracts;
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private IList<LendingRate> lendingRates;
        private IList<ContractState> _contractStates;
        #endregion


        #region constructor
        public ContractViewModel(IContractService contractService, IDialogService _dialogService)
        {
            Contracts = new List<Business.Models.Contract>();
            this._contractService = contractService;
            this._dialogService = _dialogService;
            LoadStartupData();
        }


        #endregion


        #region properties
        public List<Business.Models.Contract> Contracts
        {
            get { return _contracts; }
            set { SetProperty(ref _contracts, value); }
        }


        public IList<LendingRate> LendingRates
        {
            get { return lendingRates; }
            set { SetProperty(ref lendingRates, value); }
        }


        public IList<ContractState> ContractStates
        {
            get { return _contractStates; }
            set { SetProperty(ref _contractStates, value); }
        }

        #endregion

        #region private methods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadContractStates();
                await TryToLoadLendingRate();
                //await TryToLoadContracts();
            }
            catch (LoadingContractStatesException loadingContractStateException)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"{loadingContractStateException.Message}{Environment.NewLine}Błąd: {loadingContractStateException.InnerException.Message}", null);
            }
            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException.Message}", null);
            }
            catch (Exception e)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}", null);
            }
        }

       

        private async Task TryToLoadContractStates()
        {
            ContractStates = await _contractService.LoadContractStates();
        }

        private async Task TryToLoadLendingRate()
        {

            LendingRates = await _contractService.LoadLendingRates();

        }

        private Task TryToLoadContracts()
        {
            throw new NotImplementedException();
        }




        #endregion

    }
}