using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Models.DropDownButtonModels;
using PawnShop.Modules.Contract.Services;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PawnShop.Modules.Contract.Windows.Views;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ClientDataViewModel : BindableBase, IRegionManagerAware
    {
        #region constructor

        public ClientDataViewModel(IContractService contractService, IDialogService dialogService, IShellService shellService, IContainerProvider containerProvider)
        {
            _contractService = contractService;
            _dialogService = dialogService;
            _shellService = shellService;
            _containerProvider = containerProvider;

            LoadClientSearchOptions();
        }

        #endregion constructor

        #region Commands

        public DelegateCommand<string> SearchClientCommand =>
            _searchClientCommand ??=
                new DelegateCommand<string>(SearchClient, CanExecuteSearchClient).ObservesProperty(() =>
                    ClientSearchComboBoxText);

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);

        public DelegateCommand AddClientCommand =>
            _addClientCommand ??= new DelegateCommand(AddClient);

        public DelegateCommand<Client> EditClientCommand =>
            _editClientCommand ??= new DelegateCommand<Client>(EditClient);



        #endregion Commands

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
        private readonly IShellService _shellService;
        private readonly IContainerProvider _containerProvider;
        private IList<ClientSearchOption> _clientSearchOptions;
        private ClientSearchOption _selectedClientSearchOption;
        private IList<Client> _searchedClients;
        private DelegateCommand<string> _searchClientCommand;
        private string _clientSearchComboBoxText;
        private Client _selectedClient;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _addClientCommand;
        private DelegateCommand<Client> _editClientCommand;

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

        private void Cancel()
        {
            _shellService.CloseShell<CreateContractWindow>();
        }

        private void AddClient()
        {
            _dialogService.ShowAddClientDialog("Rejestracja nowego klienta", dialogResult =>
            {

            });
        }

        private void EditClient(Client client)
        {
            _dialogService.ShowAddClientDialog("Rejestracja nowego klienta", dialogResult =>
            {

            }, client);
        }

        #endregion commandMethods
    }
}