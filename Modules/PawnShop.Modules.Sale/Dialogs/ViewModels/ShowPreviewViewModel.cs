using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PawnShop.Modules.Sale.Dialogs.ViewModels
{
    public class ShowPreviewViewModel : BindableBase, IDialogAware
    {
        #region PrivateMembers

        private string _title;

        #endregion

        #region Constructor

        public ShowPreviewViewModel()
        {

        }

        #endregion


        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion

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
        }

        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware
    }
}
