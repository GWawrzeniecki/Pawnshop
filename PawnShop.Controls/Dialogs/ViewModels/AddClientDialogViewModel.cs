using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.Validators;
using PawnShop.Core.Enums;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PawnShop.Controls.Dialogs.ViewModels
{
    public class AddClientDialogViewModel : ViewModelBase<AddClientDialogViewModel>, IDialogAware
    {

        #region private members

        private Client _client;
        private string _title;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _createClientCommand;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
        private ClientMode _mode;
        private DelegateCommand _updateClientCommand;
        private Visibility _createClientButtonVisibility;
        private Visibility _updateClientButtonVisibility;
        private string _city;
        private string _country;
        private IList<Country> _countries;
        private Country _selectedCountry;
        private IList<City> _cities;
        private City _selectedCity;
        private string _firstName;
        private string _lastName;
        private string _street;
        private string _houseNumber;
        private string _apartmentNumber;
        private string _postCode;
        private DateTime? _birthDate;
        private string _pesel;
        private string _idCardNumber;
        private DateTime? _validityDateIdCard;

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

        public string City
        {
            get => _city;
            set
            {
                SetProperty(ref _city, value);
                SelectedCity ??= new City { City1 = City, CountryId = SelectedCountry?.CountryId ?? 0 };
                if (SelectedCity.CityId == 0)
                    SelectedCity.City1 = value;
            }
        }

        public string Country
        {
            get => _country;
            set
            {
                SetProperty(ref _country, value);
                SelectedCountry ??= new Country { Country1 = Country };
                if (SelectedCountry.CountryId == 0)
                    SelectedCountry.Country1 = value;
            }
        }

        public IList<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public IList<City> Cities
        {
            get => _cities;
            set => SetProperty(ref _cities, value);
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set => SetProperty(ref _selectedCountry, value);
        }

        public City SelectedCity
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);
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

        public AddClientDialogViewModel(AddClientValidator addClientValidator, IMapper mapper, IUnitOfWork unitOfWork, IContainerProvider containerProvider, IMessageBoxService messageBoxService) : base(addClientValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
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
            _unitOfWork.Dispose();
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Mode = parameters.GetValue<ClientMode>("mode");
            Client = parameters.TryGetValue("client", out Client client) ? client : new()
            {
                ClientNavigation = new Person { Address = new Address(), Client = new Client() }
            };
            MapClientToVmBasedOnClientMode();
            var success = await LoadStartupData();
            if (!success) return;
            MapCountryCityBasedOnClientMode();
            SetButtonBasedOnClientMode();
            AttachClient();
            AttachCountriesBasedOnClientMode();
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
                _messageBoxService.Show("Pomyślnie utworzono klienta.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters { { "client", Client } }));
            }
            catch (CreateClientException e)
            {
                _messageBoxService.ShowError(
                    $"{e.Message}.{Environment.NewLine}Błąd: {e.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {

                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async void UpdateClient()
        {
            try
            {
                await TryToUpdateClient();
                _messageBoxService.Show("Pomyślnie zapisano zmiany.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters { { "client", Client } }));
            }
            catch (UpdateClientException e)
            {
                _messageBoxService.ShowError(
                    $"{e.Message}.{Environment.NewLine}Błąd: {e.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {

                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task<bool> LoadStartupData()
        {
            var success = false;

            try
            {
                await TryToLoadStartupData();
                success = true;
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Wystąpił problem podczas pobierania listy krajów i miast.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
                RequestClose.Invoke(new DialogResult(ButtonResult.Abort));
            }

            return success;
        }

        private async Task TryToLoadStartupData()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            Countries = (await unitOfWork.CountryRepository.GetAsync()).ToList();
            Cities = (await unitOfWork.CityRepository.GetAsync()).ToList();
        }

        #endregion

        #region private methods

        private async Task TryToCreateClient()
        {
            try
            {
                MapVmToClient();
                ApplyNewCountryCityToClient();
                await _unitOfWork.ClientRepository.InsertAsync(Client);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CreateClientException("Wystąpił problem podczas dodawania nowego klienta.", e);
            }
        }

        private async Task TryToUpdateClient()
        {

            try
            {
                MapVmToClient();
                ApplyNewCountryCityToClient();
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new UpdateClientException("Wystąpił problem podczas aktualizacji danych klienta.", e);
            }

        }
        private bool CanExecuteCreateOrUpdateClient()
        {
            return !HasErrors;
        }

        private void SetButtonBasedOnClientMode()
        {
            if (Mode == ClientMode.CreateClient)
            {
                CreateClientButtonVisibility = Visibility.Visible;
            }
            else
            {
                UpdateClientButtonVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Applies Country City to Client from current DB Context
        /// Otherwise they won't be selected in ComboBox and will be doubled in db context
        /// </summary>
        private void ApplyCountryCityToClient()
        {
            Client.ClientNavigation.Address.Country = Countries.First(c =>
                c.CountryId == Client.ClientNavigation.Address.Country.CountryId);
            Client.ClientNavigation.Address.City = Cities.First(c =>
                c.CityId == Client.ClientNavigation.Address.City.CityId);
            Client.ClientNavigation.Address.City.Country = Client.ClientNavigation.Address.Country;
        }

        private void MapCountryCityToVmFromClient()
        {
            SelectedCountry = Client.ClientNavigation.Address.Country;
            SelectedCity = Client.ClientNavigation.Address.City;
        }

        private void AttachClient()
        {
            _unitOfWork.ClientRepository.Attach(Client);
        }

        private void AttachCountriesBasedOnClientMode()
        {
            if (Mode == ClientMode.CreateClient)
            {
                foreach (var country in Countries)
                {
                    _unitOfWork.CountryRepository.Attach(country);
                }
            }
            else
            {
                foreach (var country in Countries.Where(c => c.CountryId != SelectedCountry.CountryId))
                {
                    _unitOfWork.CountryRepository.Attach(country);
                }
            }

        }

        private void ApplyNewCountryCityToClient()
        {
            SelectedCity.Country = SelectedCountry;
            SelectedCity.CountryId = SelectedCountry.CountryId;
            Client.ClientNavigation.Address.City = SelectedCity;
            Client.ClientNavigation.Address.Country = SelectedCountry;
            Client.ClientNavigation.Address.CountryId = SelectedCountry.CountryId;
            Client.ClientNavigation.Address.CityId = SelectedCity.CityId;
        }
        private void MapClientToVmBasedOnClientMode()
        {
            if (Mode == ClientMode.UpdateClient)
                _mapper.Map(Client, this);
        }

        private void MapVmToClient()
        {
            _mapper.Map(this, Client);
        }

        private void MapCountryCityBasedOnClientMode()
        {
            if (Mode == ClientMode.UpdateClient)
            {
                ApplyCountryCityToClient();
                MapCountryCityToVmFromClient();
            }
        }

        #endregion
    }
}