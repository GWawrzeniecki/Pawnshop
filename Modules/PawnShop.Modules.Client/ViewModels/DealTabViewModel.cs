using PawnShop.Business.Models;
using PawnShop.Core.ViewModel;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Client.Events;
using PawnShop.Services.Interfaces;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Client.ViewModels
{
    public class DealTabViewModel : BindableBase, ITabItemViewModel
    {
        #region PrivateMembers

        private IList<Contract> _contracts;
        private readonly IContractService _contractService;
        private readonly IMessageBoxService _messageBoxService;

        #endregion

        #region Constructor

        public DealTabViewModel(IEventAggregator eventAggregator, IContractService contractService, IMessageBoxService messageBoxService)
        {
            _contractService = contractService;
            _messageBoxService = messageBoxService;
            Header = "Umowy";
            Contracts = new List<Contract>();
            eventAggregator.GetEvent<SelectedClientChangedEvent>().Subscribe(LoadContracts);
        }

        #endregion

        #region PublicProperties

        public IList<Contract> Contracts
        {
            get => _contracts;
            set => SetProperty(ref _contracts, value);
        }

        #endregion

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion

        #region SelectedClientChangedEvent

        private async void LoadContracts(Business.Models.Client client)
        {
            try
            {
                await TryToLoadContracts(client);
            }
            catch (LoadingContractsException loadingContractsException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        #endregion

        #region PrivateMethods

        private async Task TryToLoadContracts(Business.Models.Client client)
        {
            Contracts = (await _contractService.LoadContracts(client, 20))
                .ToList();
        }

        #endregion
    }
}