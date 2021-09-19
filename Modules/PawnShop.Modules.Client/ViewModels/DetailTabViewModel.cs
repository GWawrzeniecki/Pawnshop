using AutoMapper;
using PawnShop.Core.ViewModel;
using PawnShop.Modules.Client.Events;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace PawnShop.Modules.Client.ViewModels
{
    public class DetailTabViewModel : BindableBase, ITabItemViewModel
    {

        #region PrivateMembers

        private string _fullName;
        private string _fullAddress;
        private DateTime? _birthDate;
        private string _pesel;
        private string _idCardNumber;
        private readonly IMapper _mapper;

        #endregion

        #region constructor

        public DetailTabViewModel(IEventAggregator eventAggregator, IMapper mapper)
        {
            _mapper = mapper;
            Header = "Dane szczegółowe";
            eventAggregator.GetEvent<SelectedClientChangedEvent>().Subscribe(LoadClient);
        }

        #endregion

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion

        #region PublicProperties

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string FullAddress
        {
            get => _fullAddress;
            set => SetProperty(ref _fullAddress, value);
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

        public string IdCardNumber
        {
            get => _idCardNumber;
            set => SetProperty(ref _idCardNumber, value);
        }

        #endregion

        #region SelectedClientChangedEvent

        private void LoadClient(Business.Models.Client client)
        {
            _mapper.Map(client, this);
        }

        #endregion

        #region PrivateMethods



        #endregion
    }
}
