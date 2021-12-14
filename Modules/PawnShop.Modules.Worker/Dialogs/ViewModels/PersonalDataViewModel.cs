using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Validators;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class PersonalDataViewModel : WorkerDialogBase
    {
        #region PrivateMembers

        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
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
        private Task _loadStartupDataTask;

        #endregion

        #region Constructor

        public PersonalDataViewModel(IMapper mapper, IContainerProvider containerProvider, PersonalDataViewModelValidator validator, IMessageBoxService messageBoxService) : base(mapper, validator)
        {
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
            Header = "Dane personalne";
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

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

        public string Pesel
        {
            get => _pesel;
            set => SetProperty(ref _pesel, value);
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

        #endregion

        #region WorkerDialogBase

        protected override void MapWorkerBossToVm()
        {
            base.MapWorkerBossToVm();
            MapCountryCityBasedOnMode();
        }

        public override void MapVmToWorkerBoss()
        {
            base.MapVmToWorkerBoss();
            ApplyNewCountryCityToWorker();
        }

        public override void AttachAdditionalContext()
        {
            AttachCountriesBasedOnMode();
        }

        #endregion

        #region PrivateMethods

        private void LoadStartupData()
        {
            try
            {
                _loadStartupDataTask = TryToLoadStartupData();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Wystąpił problem podczas pobierania listy krajów i miast.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadStartupData()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            Countries = (await unitOfWork.CountryRepository.GetAsync()).ToList();
            Cities = (await unitOfWork.CityRepository.GetAsync()).ToList();
        }

        private async void MapCountryCityBasedOnMode()
        {
            switch (WorkerTabControlRegionContext.WorkerDialogMode)
            {
                case WorkerDialogMode.Show:
                case WorkerDialogMode.Edit:
                    await _loadStartupDataTask;
                    ApplyCountryCityToWorkerBoss();
                    MapCountryCityToVmFromWorkerBoss();
                    break;
            }
        }

        /// <summary>
        /// Applies Country City to WorkerBoss from current DB Context
        /// Otherwise they won't be selected in ComboBox and will be doubled in db context
        /// </summary>
        private void ApplyCountryCityToWorkerBoss()
        {
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country = Countries.First(c =>
                c.CountryId == WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country.CountryId);
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.City = Cities.First(c =>
                c.CityId == WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.City.CityId);
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.City.Country = WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country;
        }

        private void MapCountryCityToVmFromWorkerBoss()
        {
            SelectedCountry = WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country;
            SelectedCity = WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.City;
        }

        private void ApplyNewCountryCityToWorker()
        {
            SelectedCity.Country = SelectedCountry;
            SelectedCity.CountryId = SelectedCountry.CountryId;
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.City = SelectedCity;
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country = SelectedCountry;
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.CountryId = SelectedCountry.CountryId;
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.CityId = SelectedCity.CityId;
        }

        private void AttachCountriesBasedOnMode()
        {
            if (WorkerTabControlRegionContext.WorkerDialogMode == WorkerDialogMode.Add)
            {
                foreach (var country in Countries)
                {
                    WorkerTabControlRegionContext.UnitOfWork.CountryRepository.Attach(country);
                }
            }
            else
            {
                foreach (var country in Countries.Where(c => c.CountryId != WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country.CountryId))
                {
                    WorkerTabControlRegionContext.UnitOfWork.CountryRepository.Attach(country);
                }
            }
        }

        #endregion

    }
}
