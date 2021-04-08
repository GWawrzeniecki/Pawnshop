using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.Models.DropDownButtonModels;
using PawnShop.Modules.Contract.Services;
using PawnShop.Modules.Contract.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ClientDataViewModel : BindableBase, IRegionManagerAware, IConfirmNavigationRequest
    {
        #region constructor

        public ClientDataViewModel(IContractService contractService, IDialogService dialogService)
        {
            _contractService = contractService;
            _dialogService = dialogService;

            LoadClientSearchOptions();
        }

        #endregion constructor

        #region Commands

        public DelegateCommand<string> SearchClientCommand =>
            _searchClientCommand ??=
                new DelegateCommand<string>(SearchClient, CanExecuteSearchClient).ObservesProperty(() =>
                    ClientSearchComboBoxText);

        public DelegateCommand GoForwardCommand =>
            _goForwardCommand ??= new DelegateCommand(GoForward);



        #endregion Commands


        #region  IConfirmNavigatonRequest

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        #endregion


        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region privateMethods

        private void LoadClientSearchOptions()
        {
            ClientSearchOptions = new List<ClientSearchOption>
            {
                new ClientSearchOption {Name = "Po nazwisku", SearchOption = Enums.ClientSearchOption.Surname},
                new ClientSearchOption {Name = "Po peselu", SearchOption = Enums.ClientSearchOption.Pesel}
            };
        }

        #endregion privateMethods

        #region privateMembers

        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private IList<ClientSearchOption> _clientSearchOptions;
        private ClientSearchOption _selectedClientSearchOption;
        private IList<Client> _searchedClients;
        private DelegateCommand<string> _searchClientCommand;
        private string _clientSearchComboBoxText;
        private Client _selectedClient;
        private IRegionNavigationJournal _journal;
        private DelegateCommand _goForwardCommand;

        #endregion privateMembers

        #region public properties

        public IList<ClientSearchOption> ClientSearchOptions
        {
            get => _clientSearchOptions;
            set => SetProperty(ref _clientSearchOptions, value);
        }

        public ClientSearchOption SelectedClientSearchOption
        {
            get => _selectedClientSearchOption;
            set
            {
                SetProperty(ref _selectedClientSearchOption, value);
                SearchClientCommand.RaiseCanExecuteChanged();
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public IList<Client> SearchedClients
        {
            get => _searchedClients;
            set => SetProperty(ref _searchedClients, value);
        }

        public string ClientSearchComboBoxText
        {
            get => _clientSearchComboBoxText;
            set => SetProperty(ref _clientSearchComboBoxText, value);
        }

        #endregion public properties

        #region commandMethods

        private async void SearchClient(string surname)
        {
            try
            {
                await TryToSearchClient(surname);
            }
            catch (SearchClientsException searchClientsException)
            {
                _dialogService.ShowNotificationDialog("Błąd",
                    $"{searchClientsException.Message}{Environment.NewLine}Błąd: {searchClientsException.InnerException?.Message}",
                    null);
            }
            catch (Exception e)
            {
                _dialogService.ShowNotificationDialog("Błąd",
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}", null);
            }
        }

        private async Task TryToSearchClient(string surname)
        {
            SearchedClients = SelectedClientSearchOption.SearchOption switch
            {
                Enums.ClientSearchOption.Surname => await _contractService.GetClientBySurname(surname),
                Enums.ClientSearchOption.Pesel => await _contractService.GetClientByPesel(surname),
                _ => throw new ArgumentOutOfRangeException(nameof(SelectedClientSearchOption.SearchOption))
            };
        }

        private bool CanExecuteSearchClient(string arg)
        {
            return !string.IsNullOrEmpty(arg) && !string.IsNullOrWhiteSpace(arg) && SelectedClientSearchOption != null;
        }

        private void GoForward()
        {
            if (_journal != null && _journal.CanGoForward)
                _journal.GoForward();
            else
            {
                RegionManager.RequestNavigate(RegionNames.ContentRegion,nameof(ContractData)); //hmi icon isnt changing TO DO
                //var view = RegionManager.Regions[RegionNames.MenuRegion].GetView(nameof(ContractDataHamburgerMenuItem));
                //RegionManager.Regions[RegionNames.MenuRegion].Activate(view); trying to navigate via hmi to get icon selected 
            }
        }

        #endregion commandMethods
    }
}