using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.Models.DropDownButtonModels;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class CreateContractClientDataViewModel : BindableBase, IRegionManagerAware, INavigationAware, IHamburgerMenuEnabled
    {
        #region privateMembers

        private readonly IDialogService _dialogService;
        private readonly IClientService _clientService;
        private readonly IMessageBoxService _messageBoxService;
        private IList<ClientSearchOption> _clientSearchOptions;
        private ClientSearchOption _selectedClientSearchOption;
        private IList<Client> _searchedClients;
        private DelegateCommand _searchClientCommand;
        private string _clientSearchComboBoxText;
        private Client _selectedClient;
        private DelegateCommand _addClientCommand;
        private DelegateCommand _editClientCommand;
        private bool _clientSearchComboBoxIsOpen;
        private Func<Task> _callBack;

        #endregion privateMembers

        #region constructor

        public CreateContractClientDataViewModel(IDialogService dialogService, IContainerProvider containerProvider, IClientService clientService, IMessageBoxService messageBoxService)
        {
            _dialogService = dialogService;
            _clientService = clientService;
            _messageBoxService = messageBoxService;
            HamburgerMenuItem = containerProvider.Resolve<CreateContractContractDataHamburgerMenuItem>();
            LoadClientSearchOptions();
        }

        #endregion constructor

        #region Commands

        public DelegateCommand SearchClientCommand =>
            _searchClientCommand ??=
                new DelegateCommand(SearchClient, CanExecuteSearchClient)
            .ObservesProperty(() => ClientSearchComboBoxText)
            .ObservesProperty(() => SelectedClientSearchOption);

        public DelegateCommand AddClientCommand =>
            _addClientCommand ??= new DelegateCommand(AddClient);

        public DelegateCommand EditClientCommand =>
            _editClientCommand ??= new DelegateCommand(EditClient, CanExecuteEditClient)
                .ObservesProperty(() => SelectedClient);

        #endregion Commands

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region privateMethods

        private void LoadClientSearchOptions()
        {
            ClientSearchOptions = new List<ClientSearchOption>
            {
                new() {Name = "Po nazwisku", SearchOption = Enums.ClientSearchOption.Surname},
                new() {Name = "Po peselu", SearchOption = Enums.ClientSearchOption.Pesel}
            };
        }

        #endregion privateMethods

        #region public properties

        public IList<ClientSearchOption> ClientSearchOptions
        {
            get => _clientSearchOptions;
            set => SetProperty(ref _clientSearchOptions, value);
        }

        public ClientSearchOption SelectedClientSearchOption
        {
            get => _selectedClientSearchOption;
            set => SetProperty(ref _selectedClientSearchOption, value);
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                SetProperty(ref _selectedClient, value);
                RaisePropertyChanged(nameof(IsNextButtonEnabled));
                (this as IHamburgerMenuEnabled).IsEnabled = IsNextButtonEnabled;
            }
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


        public bool ClientSearchComboBoxIsOpen
        {
            get => _clientSearchComboBoxIsOpen;
            set => SetProperty(ref _clientSearchComboBoxIsOpen, value);
        }

        public bool IsNextButtonEnabled => SelectedClient != null;


        #endregion public properties

        #region commandMethods

        private async void SearchClient()
        {
            try
            {
                await TryToSearchClient(ClientSearchComboBoxText);
            }
            catch (SearchClientsException searchClientsException)
            {
                _messageBoxService.ShowError(
                    $"{searchClientsException.Message}{Environment.NewLine}Błąd: {searchClientsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToSearchClient(string value)
        {
            SearchedClients = SelectedClientSearchOption.SearchOption switch
            {
                Enums.ClientSearchOption.Surname => await _clientService.GetClientBySurname(value),
                Enums.ClientSearchOption.Pesel => await _clientService.GetClientByPesel(value),
                _ => throw new ArgumentOutOfRangeException(nameof(SelectedClientSearchOption.SearchOption))
            };

            if (SearchedClients?.Count == 1)
                SelectedClient = SearchedClients.First();
            else
                ClientSearchComboBoxIsOpen = true;
        }

        private bool CanExecuteSearchClient()

        {
            return !string.IsNullOrEmpty(ClientSearchComboBoxText) && !string.IsNullOrWhiteSpace(ClientSearchComboBoxText) && SelectedClientSearchOption != null;
        }



        private void AddClient()
        {
            _dialogService.ShowAddClientDialog("Rejestracja nowego klienta", ClientMode.CreateClient, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    SelectedClient = null;
                    SelectedClient = dialogResult.Parameters.GetValue<Client>("client");
                }

            });
        }

        private void EditClient()
        {
            _dialogService.ShowAddClientDialog("Rejestracja nowego klienta", ClientMode.UpdateClient, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    SelectedClient = null;
                    SelectedClient = dialogResult.Parameters.GetValue<Client>("client");
                }
            }, SelectedClient);
        }

        private bool CanExecuteEditClient() => SelectedClient != null;

        #endregion commandMethods

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _callBack = navigationContext.Parameters.GetValue<Func<Task>>("CallBack");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("DealMaker", SelectedClient);
            navigationContext.Parameters.Add("CallBack", _callBack);
        }

        #endregion

        #region IHamburgerMenuEnabled

        public HamburgerMenuItemBase HamburgerMenuItem { get; set; }


        #endregion

    }
}