using PawnShop.Business.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
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

        #endregion public properties


        #region commands

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);



        public DelegateCommand CreateClientCommand =>
            _createClientCommand ??= new DelegateCommand(CreateClient);



        #endregion

        #region constructor

        public AddClientDialogViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            Client = parameters.TryGetValue("client", out Client client) ? client : new Client();
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
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters { { "client", Client } }));
            }
            catch (Exception e)
            {

            }
        }

        #endregion

        #region private methods

        private async Task TryToCreateClient()
        {
            await _unitOfWork.ClientRepository.CreateClientAsync(Client);

        }
        #endregion
    }
}