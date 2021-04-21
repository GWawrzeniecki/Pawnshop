using PawnShop.Business.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using BespokeFusion;
using PawnShop.Core.Enums;
using PawnShop.Services.DataService;
using Prism.Commands;

namespace PawnShop.Modules.Contract.Dialogs.ViewModels
{
    public class AddClientDialogViewModel : BindableBase, IDialogAware
    {
        #region private members

        private Client _client;
        private string _title;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _createClientCommand;
        private readonly IUnitOfWork _unitOfWork;
        private ClientMode _mode;
        private DelegateCommand _updateClientCommand;
        private Visibility _createClientButtonVisibility;
        private Visibility _updateClientButtonVisibility;

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

        #endregion public properties


        #region commands

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);


        public DelegateCommand CreateClientCommand =>
            _createClientCommand ??= new DelegateCommand(CreateClient);

        public DelegateCommand UpdateClientCommand =>
            _updateClientCommand ??= new DelegateCommand(UpdateClient);

        #endregion

        #region constructor

        public AddClientDialogViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CreateClientButtonVisibility = Visibility.Hidden;
            UpdateClientButtonVisibility = Visibility.Hidden;
        }

        #endregion constructor

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
                CreateClientButtonVisibility = Visibility.Visible;
            else
                UpdateClientButtonVisibility = Visibility.Visible;
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
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił błąd podczas dodawania nowego klienta.{Environment.NewLine}Błąd: {e.Message}",
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
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił błąd podczas edycji klienta.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        #endregion

        #region private methods

        private async Task TryToCreateClient()
        {
            await _unitOfWork.ClientRepository.CreateClientAsync(Client);
        }


        private async Task TryToUpdateClient()
        {
            await _unitOfWork.ClientRepository.UpdateClientAsync(Client);
        }

        #endregion
    }
}