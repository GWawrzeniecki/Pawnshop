using AutoMapper;
using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PawnShop.Modules.Contract.Dialogs.ViewModels
{
    public class AddClientDialogViewModel : ViewModelBase<AddClientDialogViewModel>, IDialogAware
    {
        #region private members

        private Client _client;
        private string _title;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _createClientCommand;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private ClientMode _mode;
        private DelegateCommand _updateClientCommand;
        private Visibility _createClientButtonVisibility;
        private Visibility _updateClientButtonVisibility;
        private string _firstName;
        private string _lastName;
        private string _street;
        private string _houseNumber;
        private string _apartmentNumber;
        private string _city;
        private string _postCode;
        private DateTime? _birthDate;
        private string _pesel;
        private string _idCardNumber;
        private DateTime? _validityDateIdCard;
        private string _country;

        #endregion private members


        #region public properties

        public Client Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }


        public Visibility CreateClientButtonVisibility
        {
            get => _createClientButtonVisibility;
            set => SetProperty(ref _createClientButtonVisibility, value);
        }


        public Visibility UpdateClientButtonVisibility
        {
            get => _updateClientButtonVisibility;
            set => SetProperty(ref _updateClientButtonVisibility, value);
        }


        public ClientMode Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
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

        public string Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }

        public string HouseNumber
        {
            get => _houseNumber;
            set => SetProperty(ref _houseNumber, value);
        }

        public string ApartmentNumber
        {
            get => _apartmentNumber;
            set => SetProperty(ref _apartmentNumber, value);
        }

        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public string Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }

        public string PostCode
        {
            get => _postCode;
            set => SetProperty(ref _postCode, value);
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public DateTime? ValidityDateIdCard
        {
            get => _validityDateIdCard;
            set => SetProperty(ref _validityDateIdCard, value);
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



        #endregion public properties


        #region commands

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);


        public DelegateCommand CreateClientCommand =>
            _createClientCommand ??=
                new DelegateCommand(CreateClient, CanExecuteCreateOrUpdateClient)
                    .ObservesProperty(() => HasErrors);


        public DelegateCommand UpdateClientCommand =>
            _updateClientCommand ??= new DelegateCommand(UpdateClient, CanExecuteCreateOrUpdateClient)
                .ObservesProperty(() => HasErrors);

        #endregion

        #region constructor

        public AddClientDialogViewModel(AddClientValidator addClientValidator, IMapper mapper, IClientService clientService) :
            base(
                addClientValidator)
        {

            _mapper = mapper;
            _clientService = clientService;
            CreateClientButtonVisibility = Visibility.Hidden;
            UpdateClientButtonVisibility = Visibility.Hidden;
        }

        #endregion constructor

        #region viewModelBase

        protected override AddClientDialogViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

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
            Title = parameters.GetValue<string>("title");
            Mode = parameters.GetValue<ClientMode>("mode");
            Client = parameters.TryGetValue("client", out Client client) ? client : new Client();

            if (Mode == ClientMode.CreateClient)
            {
                CreateClientButtonVisibility = Visibility.Visible;
            }
            else
            {
                UpdateClientButtonVisibility = Visibility.Visible;
                _mapper.Map(Client, this);
            }
        }


        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region commandMethods

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async void CreateClient()
        {
            try
            {
                await TryToCreateClient();
                MaterialMessageBox.Show("Pomyślnie utworzono klienta.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters { { "client", Client } }));
            }
            catch (CreateClientException e)
            {
                MaterialMessageBox.ShowError(
                    $"{e.Message}.{Environment.NewLine}Błąd: {e.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {

                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async void UpdateClient()
        {
            try
            {
                await TryToUpdateClient();
                MaterialMessageBox.Show("Pomyślnie zapisano zmiany.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters { { "client", Client } }));
            }
            catch (UpdateClientException e)
            {
                MaterialMessageBox.ShowError(
                    $"{e.Message}.{Environment.NewLine}Błąd: {e.InnerException?.Message}",
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

        #region private methods

        private async Task TryToCreateClient()
        {
            Client = _mapper.Map(this, Client);
            Client = await _clientService.CreateClient(Client);

        }


        private async Task TryToUpdateClient()
        {
            Client = _mapper.Map(this, Client);
            Client = await _clientService.UpdateClient(Client);

        }

        private bool CanExecuteCreateOrUpdateClient()
        {
            return !HasErrors;
        }

        #endregion
    }
}