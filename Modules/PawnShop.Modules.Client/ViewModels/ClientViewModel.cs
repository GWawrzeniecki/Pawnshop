using AutoMapper;
using BespokeFusion;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using PawnShop.Core.Events;
using PawnShop.Core.Models.DropDownButtonModels;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Client.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Client.ViewModels
{
    public class ClientViewModel : ViewModelBase<ClientViewModel>
    {
        #region PrivateMembers

        private readonly IDialogService _dialogService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private DelegateCommand _createClientCommand;
        private DelegateCommand<Business.Models.Client> _editClientCommand;
        private string _firstName;
        private string _lastName;
        private string _contractNumber;
        private string _pesel;
        private string _idCardNumber;
        private string _street;
        private DelegateCommand _refreshCommand;
        private IList<Business.Models.Client> _clients;
        private Business.Models.Client _selectedClient;
        private IList<RefreshButtonOption> _refreshButtonOptions;
        private DelegateCommand<object> _refreshButtonCommand;

        #endregion

        #region Constructor

        public ClientViewModel(IDialogService dialogService, IClientService clientService, IMapper mapper, IEventAggregator eventAggregator, ClientViewModelValidator clientViewModelValidator) : base(clientViewModelValidator)
        {
            _dialogService = dialogService;
            _clientService = clientService;
            _mapper = mapper;
            _eventAggregator = eventAggregator;
            Clients = new List<Business.Models.Client>();
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

        public IList<Business.Models.Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public Business.Models.Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                SetProperty(ref _selectedClient, value);

                if (value is not null)
                    _eventAggregator.GetEvent<SelectedClientChangedEvent>().Publish(value);
            }
        }
        public IList<RefreshButtonOption> RefreshButtonOptions
        {
            get => _refreshButtonOptions;
            set => SetProperty(ref _refreshButtonOptions, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string ContractNumber
        {
            get => _contractNumber;
            set => SetProperty(ref _contractNumber, value);
        }

        public string Pesel
        {
            get => _pesel;
            set => SetProperty(ref _pesel, value);
        }

        public string IdCardNumber
        {
            get => _idCardNumber;
            set => SetProperty(ref _idCardNumber, value);
        }

        public string Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }

        #endregion

        #region viewModelBase

        protected override ClientViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region Commands

        public DelegateCommand CreateClientCommand =>
            _createClientCommand ??= new DelegateCommand(CreateClient);

        public DelegateCommand<Business.Models.Client> EditClientCommand =>
            _editClientCommand ??= new DelegateCommand<Business.Models.Client>(EditClient, CanExecuteEditClient)
                .ObservesProperty(() => SelectedClient);

        public DelegateCommand RefreshCommand =>
            _refreshCommand ??= new DelegateCommand(RefreshDataGrid);

        public DelegateCommand<object> RefreshButtonOptionCommand =>
            _refreshButtonCommand ??= new DelegateCommand<object>(SetRefreshButtonOption);

        #endregion Commands

        #region CommandMethods

        private void CreateClient()
        {
            _dialogService.ShowAddClientDialog("Rejestracja nowego klienta", ClientMode.CreateClient, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    SelectedClient = null;
                    SelectedClient = dialogResult.Parameters.GetValue<Business.Models.Client>("client");
                }

            });
        }

        private void EditClient(Business.Models.Client client)
        {
            _dialogService.ShowAddClientDialog("Rejestracja nowego klienta", ClientMode.UpdateClient, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    SelectedClient = null;
                    SelectedClient = dialogResult.Parameters.GetValue<Business.Models.Client>("client");
                }
            }, client);
        }

        private bool CanExecuteEditClient(Business.Models.Client arg)
        {
            return SelectedClient is not null;
        }

        private async void RefreshDataGrid()
        {
            try
            {
                var clientQueryData = _mapper.Map<ClientQueryData>(this);
                await TryToRefreshDataGrid(clientQueryData);
            }
            catch (SearchClientsException loadingContractsException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        #endregion

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadStartupData();
                LoadRefreshButtonOptions();
            }
            catch (LoadingClientsException loadingClientsException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingClientsException.Message}{Environment.NewLine}Błąd: {loadingClientsException.InnerException?.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadStartupData()
        {
            Clients = await _clientService.GetClients(100);
        }

        private void LoadRefreshButtonOptions()
        {
            RefreshButtonOptions = new List<RefreshButtonOption>
            {
                new() {Name = "Wyczyść filtr", RefreshOption = RefreshOptions.Clean},
                new() {Name = "Wyczyść filtr i odśwież", RefreshOption = RefreshOptions.CleanAndRefresh}
            };
        }

        private void SetRefreshButtonOption(object refreshOption)
        {
            switch (refreshOption)
            {
                case RefreshOptions.Clean:
                    CleanSearchProperties();
                    break;

                case RefreshOptions.CleanAndRefresh:
                    CleanSearchProperties();
                    RefreshCommand.Execute();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(refreshOption), refreshOption, null);
            }
        }

        private void CleanSearchProperties()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Pesel = string.Empty;
            Street = string.Empty;
            IdCardNumber = string.Empty;
            ContractNumber = string.Empty;
        }

        private async Task TryToRefreshDataGrid(ClientQueryData clientQueryData)
        {
            Clients = await _clientService.GetClients(clientQueryData, 100);
        }

        #endregion
    }
}
