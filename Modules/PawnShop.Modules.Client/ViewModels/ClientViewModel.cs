using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PawnShop.Modules.Client.ViewModels
{
    public class ClientViewModel : BindableBase
    {
        #region PrivateMembers

        private readonly IDialogService _dialogService;
        private DelegateCommand _createClientCommand;
        private DelegateCommand<Business.Models.Client> _editClientCommand;

        #endregion

        #region Constructor

        public ClientViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #endregion

        #region PublicProperties

        public Business.Models.Client SelectedClient { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CreateClientCommand =>
            _createClientCommand ??= new DelegateCommand(CreateClient);

        public DelegateCommand<Business.Models.Client> EditClientCommand =>
            _editClientCommand ??= new DelegateCommand<Business.Models.Client>(EditClient, CanExecuteEditClient)
                .ObservesProperty(() => SelectedClient);

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

        #endregion
    }
}
